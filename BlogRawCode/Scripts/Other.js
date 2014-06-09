$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('.scrollToTop').fadeIn(1000);
        } else {
            $('.scrollToTop').fadeOut(500);
        }
    });

    $('.scrollToTop').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1200);
        return false;
    });

    //-------------------
    $(window).scroll(function () {
        if ($(this).scrollTop() < 200) {
            $('.scrollToBottom').fadeIn(1000);
        } else {
            $('.scrollToBottom').fadeOut(500);
        }
    });

    $('.scrollToBottom').click(function () {
        $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    });

    //--------------------
    $('#SearchIcon').click(function () {
        $("#searchBox input").animate({ width: 'toggle' }, 200);
    });

    //--------------------
    $('#showComment').click(function () {
        $("#commentContainer").animate({ height: 'toggle' }, 500);
        $('#showComment .glyphicon').toggleClass('glyphicon-circle-arrow-down glyphicon-circle-arrow-up');
    });
    //--------------------
    $('.tabOpener').click(function () {
        $('.tabOpener .glyphicon').toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
        $('.editTab').animate({ height: 'toggle' }, 200);
    });
});

