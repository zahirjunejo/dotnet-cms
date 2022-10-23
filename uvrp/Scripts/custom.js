$(window).scroll(function () {
    var scroll = $(window).scrollTop();
    if (scroll >= 70) {
        $(".mainNavbar").addClass("addBG");
        $(".navbar-brand img").attr("width", "178");
    } else {
        $(".mainNavbar").removeClass("addBG");
        $(".navbar-brand img").attr("width", "248");
    }
});

const $dropdown = $(" .dropdown");
const $dropdownToggle = $(".dropdown-toggle");
const $dropdownMenu = $(".dropdown-menu");
const showClass = "show";

$(window).on("load resize", function () {
    if (this.matchMedia("(min-width: 768px)").matches) {
        $dropdown.hover(
            function () {
                const $this = $(this);
                $this.addClass(showClass);
                $this.find($dropdownToggle).attr("aria-expanded", "true");
                $this.find($dropdownMenu).addClass(showClass);
            },
            function () {
                const $this = $(this);
                $this.removeClass(showClass);
                $this.find($dropdownToggle).attr("aria-expanded", "false");
                $this.find($dropdownMenu).removeClass(showClass);
            }
        );
    } else {
        $dropdown.off("mouseenter mouseleave");
    }
});

$('.dropdown-toggle').click(function () { var location = $(this).attr('href'); window.location.href = location; return false; });

$("#scrollMore").click(function () {
    $("html, body").animate({
        scrollTop: $("#moveScroll").offset().top - 100
    }, 2000);
});

$(function () {
    
    if ($('#addClass').val() == 1) {
        $('.wrapper').addClass('with_sidebar');
    }

});

$(function () {
    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if (
            $(this)
                .parent()
                .hasClass("active")
        ) {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .parent()
                .removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .next(".sidebar-submenu")
                .slideDown(200);
            $(this)
                .parent()
                .addClass("active");
        }
    });

    $("#close-sidebar").click(function () {
        $(".sidebar-page-wrapper").removeClass("toggled");
    });
    $("#show-sidebar").click(function () {
        $(".sidebar-page-wrapper").addClass("toggled");
    });




});