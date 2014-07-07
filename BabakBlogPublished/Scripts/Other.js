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
    //--------------------
    $('.carousel').carousel();
    //--------------------
    $('.shareButton').click(function () {
        $('.shareIcons').animate({ width: 'toggle' }, 200);
    });
    //------- FishEye -------
    $('.fishHead').click(function () {
        $(".fishBody").toggle("pulsate");
    });

    $('#fishButton').click(function () {
        $('html, body').animate({ scrollTop: 199 }, 100);
        $('.fishBody').effect("shake");
    });

    $("#sortable").sortable();
    //------- menu --------
    $('.menuButton').click(function () {
        $(".menu").toggle("slide", 200);
        $(".menuButton").switchClass("mbp-1", "mbp-2",-1);
    });

    $('#content').click(function () {
        $(".menu").fadeOut(-1);
        $(".menuButton").switchClass("mbp-2", "mbp-1", -1);
    });

    $('.navbar').click(function () {
        $(".menu").fadeOut(-1);
        $(".menuButton").switchClass("mbp-2", "mbp-1", -1);
    });

    $('#footer').click(function () {
        $(".menu").fadeOut(-1);
        $(".menuButton").switchClass("mbp-2", "mbp-1", -1);
    });
    //------------------
    $('.titleImage-1').click(function () {
        $(this).attr('src', '/Content/Images/tv.png');
    });
    //------------------
    $('.blackOverlay').click(function () {
        $(".menuArrow").fadeOut(50);
        $(".blackOverlay").fadeOut(100);
    });
    $('.menuButton').click(function () {
        $(".menuArrow").fadeOut(50);
        $(".blackOverlay").fadeOut(100);
    });
    //------------------
    $('#EL').click(function () {
        alert('در این قسمت محتویات فولدر "Content" بر روی سرور را مشاهده می کنید.\n\nبرای فرستادن فایل روی سرور می توانید از دکمه "آپلود" استفاده کنید.');
    });
    //-------------------
    
   
});

