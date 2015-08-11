$(function() {
    $('.rollover').hover(function() {
        var currentImg = $(this).attr('src');
        $(this).attr('src', $(this).attr('data-hover'));
        $(this).attr('data-hover', currentImg);
    }, function() {
        var currentImg = $(this).attr('src');
        $(this).attr('src', $(this).attr('data-hover'));
        $(this).attr('data-hover', currentImg);
    });
});