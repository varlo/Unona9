using System;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Xml;
using Timer = System.Timers.Timer;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AspNetDating.Classes
{
    public class GoogleSitemaps
    {
        private enum Frequency
        {
            daily,
            weekly,
            monthly,
            yearly
        }

        private class SitemapNode
        {
            public string Location { get; set; }
            public DateTime? LastModified { get; set; }
            public Frequency ChangeFrequency { get; set; }
            public decimal Priority { get; set; }
        }

        private static Timer timerGenerateSitemap;
        private static bool generateSitemapsLock = false;

        internal static void InitializeTimers()
        {
            timerGenerateSitemap = new Timer();
            timerGenerateSitemap.AutoReset = true;
            timerGenerateSitemap.Interval = TimeSpan.FromHours(6).TotalMilliseconds;
            timerGenerateSitemap.Elapsed += timerGenerateSitemap_Elapsed;
            timerGenerateSitemap.Start();
        }

        private static void timerGenerateSitemap_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(DBSettings.Get("GoogleSitemaps_LastGenerated", DateTime.Now)) >=
                TimeSpan.FromDays(7))
            {
                ThreadPool.QueueUserWorkItem(AsyncGenerateSitemaps);
                DBSettings.Set("GoogleSitemaps_LastGenerated", DateTime.Now);
            }
        }

        private static void AsyncGenerateSitemaps(object data)
        {
            if (generateSitemapsLock)
                return;

            try
            {
                generateSitemapsLock = true;

                GenerateSitemaps();
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AsyncGenerateSitemaps", err);
            }
            finally
            {
                generateSitemapsLock = false;
            }
        }

        private static void GenerateSitemaps()
        {
            var nodes = new List<SitemapNode>();

            #region Basic pages

            nodes.AddRange(new[]{
                         new SitemapNode
                         {
                             Location = Config.Urls.Home,
                             ChangeFrequency = Frequency.weekly,
                             Priority = 1.0M
                         },
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/Login.aspx",
                             ChangeFrequency = Frequency.monthly,
                             Priority = 0.1M
                         },
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/Register.aspx",
                             ChangeFrequency = Frequency.monthly,
                             Priority = 0.8M
                         },
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/Search2.aspx",
                             ChangeFrequency = Frequency.monthly,
                             Priority = 0.1M
                         }
            });

            if (Config.Ratings.EnableProfileRatings)
                nodes.Add(
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/TopUsers.aspx",
                             ChangeFrequency = Frequency.daily,
                             Priority = 0.7M
                         }
                    );

            if (Config.Ratings.EnablePhotoRatings)
                nodes.Add(
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/TopPhotos.aspx",
                             ChangeFrequency = Frequency.daily,
                             Priority = 0.7M
                         }
                    );

            #endregion

            #region Photo contests

            if (Config.Ratings.EnablePhotoContests)
            {
                nodes.Add(
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/PhotoContests.aspx",
                             ChangeFrequency = Frequency.weekly,
                             Priority = 0.5M
                         }
                    );
                PhotoContest[] contests = PhotoContest.Load(null);
                foreach (PhotoContest contest in contests)
                {
                    if (contest.DateEnds.HasValue && contest.DateEnds.Value < DateTime.Now)
                    {
                        nodes.Add(
                                 new SitemapNode
                                 {
                                     Location = Config.Urls.Home + "/PhotoContest.aspx?cid=" + contest.Id,
                                     ChangeFrequency = Frequency.yearly,
                                     Priority = 0.2M
                                 }
                            );
                    }
                    else
                    {
                        nodes.Add(
                                 new SitemapNode
                                 {
                                     Location = Config.Urls.Home + "/PhotoContest.aspx?cid=" + contest.Id,
                                     ChangeFrequency = Frequency.daily,
                                     Priority = 0.5M
                                 }
                            );
                    }
                }
            }

            #endregion

            Language[] langs = Language.FetchAll();

            #region News

            foreach (Language lang in langs)
            {
                if (!lang.Active) continue;
                foreach (News news in News.FetchAsArray(lang.Id))
                {
                    nodes.Add(
                             new SitemapNode
                             {
                                 Location = Config.Urls.Home + "/News.aspx?id=" + news.ID,
                                 LastModified = news.PublishDate,
                                 ChangeFrequency = Frequency.monthly,
                                 Priority = 0.5M
                             }
                        );
                }
            }

            #endregion

            #region Content pages

            foreach (Language lang in langs)
            {
                if (!lang.Active) continue;
                foreach (ContentPage page in ContentPage.FetchContentPages(lang.Id, ContentPage.eSortColumn.None))
                {
                    if (page.URL != null || page.VisibleFor == ContentPage.eVisibility.LoggedOnUsers
                        || page.VisibleFor == ContentPage.eVisibility.Paid
                        || page.VisibleFor == ContentPage.eVisibility.Unpaid
                        || (!page.HeaderPosition.HasValue && !page.FooterPosition.HasValue)) continue;

                    nodes.Add(
                             new SitemapNode
                             {
                                 Location = UrlRewrite.CreateContentPageUrl(page.ID),
                                 ChangeFrequency = Frequency.weekly,
                                 Priority = 0.5M
                             }
                        );
                }
            }

            #endregion

            #region Groups

            if (Config.Groups.EnableGroups)
            {
                nodes.Add(
                         new SitemapNode
                         {
                             Location = Config.Urls.Home + "/Groups.aspx",
                             ChangeFrequency = Frequency.weekly,
                             Priority = 0.5M
                         }
                    );

                foreach (Group group in Group.Fetch(true, Group.eSortColumn.None))
                {
                    nodes.Add(
                             new SitemapNode
                             {
                                 Location = UrlRewrite.CreateShowGroupUrl(group.ID.ToString()),
                                 ChangeFrequency = Frequency.weekly,
                                 Priority = 0.5M
                             }
                        );

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers))
                        nodes.Add(
                                 new SitemapNode
                                 {
                                     Location = UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString()),
                                     ChangeFrequency = Frequency.weekly,
                                     Priority = 0.5M
                                 }
                            );

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers))
                    {
                        nodes.Add(
                                 new SitemapNode
                                 {
                                     Location = UrlRewrite.CreateShowGroupTopicsUrl(group.ID.ToString()),
                                     ChangeFrequency = Frequency.daily,
                                     Priority = 0.8M
                                 }
                            );

                        foreach (GroupTopic topic in GroupTopic.FetchByGroup(group.ID))
                        {
                            nodes.Add(
                                     new SitemapNode
                                     {
                                         Location = UrlRewrite.CreateShowGroupTopicsUrl(group.ID.ToString(), topic.ID.ToString()),
                                         LastModified = topic.DateUpdated,
                                         ChangeFrequency = Frequency.weekly,
                                         Priority = 0.6M
                                     }
                                );
                        }
                    }

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers))
                    {
                        nodes.Add(
                                 new SitemapNode
                                 {
                                     Location = UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString()),
                                     ChangeFrequency = Frequency.daily,
                                     Priority = 0.8M
                                 }
                            );

                        foreach (GroupEvent groupEvent in GroupEvent.FetchByGroupID(group.ID))
                        {
                            nodes.Add(
                                     new SitemapNode
                                     {
                                         Location = UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString(), groupEvent.ID.ToString()),
                                         ChangeFrequency = Frequency.weekly,
                                         Priority = 0.6M
                                     }
                                );
                        }
                    }
                }
            }

            #endregion

            #region Profiles

            BasicSearch search = new BasicSearch();
            search.Deleted = false;
            search.Active = true;
            search.hasAnswer_isSet = false;
            search.hasPhoto_isSet = false;

            foreach (string username in search.GetResults().Usernames)
            {
                nodes.Add(
                         new SitemapNode
                         {
                             Location = UrlRewrite.CreateShowUserUrl(username),
                             ChangeFrequency = Frequency.weekly,
                             Priority = 0.5M
                         }
                    );

                if (Config.Misc.EnableBlogs)
                {
                    Blog blog = Blog.Load(username);
                    if (blog != null)
                    {
                        foreach (BlogPost post in BlogPost.Fetch(blog.Id))
                        {
                            nodes.Add(
                                    new SitemapNode
                                    {
                                        Location = UrlRewrite.CreateShowUserBlogUrl(username, post.Id),
                                        ChangeFrequency = Frequency.monthly,
                                        Priority = 0.6M
                                    }
                                );
                        }
                    }
                }
            }

            #endregion

            var xml = new XElement("urlset",
                new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                from node in nodes
                select new XElement("url",
                    new XElement("loc", node.Location),
                    node.LastModified.HasValue ?
                    new XElement("lastmod", node.LastModified.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)) : null,
                    new XElement("changefreq", node.ChangeFrequency.ToString()),
                    new XElement("priority", node.Priority)
                )
            );

            XmlTextWriter writer = new XmlTextWriter(Config.Directories.Home + "/sitemap.xml",
                System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = '\t';
            writer.WriteStartDocument();
            xml.WriteTo(writer);
            writer.Close();
        }
    }
}