let is_open = true
const min_width = $(".side-nav__toggle").css("width")

export function ready () {
    $(".side-nav__toggle").click(function () {is_open ? hide_nav_bar() : show_nav_bar()})
    $(window).resize(hide_nav_bar)
}

function hide_nav_bar() {
    $(".side-nav").css("width", min_width)
    $(".presentation").css("margin-left", min_width)

    is_open = false
}

function show_nav_bar() {
    // max-lenght depends on a screen size, so should be checked every time triggered
    let max_width = $(".side-nav").css("max-width")

    $(".side-nav").css("width", max_width)
    $(".presentation").css("margin-left", max_width)

    is_open = true
}