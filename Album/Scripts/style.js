// Hide or show the menu in mobile display
$(document).ready(function () {
    $(".navbar-burger").click(function () {
        $(".navbar-burger").toggleClass("is-active")
        $(".navbar-menu").toggleClass("is-active")
    })
})

// Change the name of the file
$(document).ready(function () {
    $(".file-input").on("change", function () {
        console.log($(this))
        if ($(this).is("#albumInput")) {
            $("#albumName").text($(this)[0].files[0].name)
        } else if ($(this).is("#stateInput")) {
            $("#stateName").text($(this)[0].files[0].name)
        }
    })
})

// Hide the unused elements in the LoadFiles view
$(document).ready(function () {
    if ($("#dictionaries").val() == "empty") {
        $("#download").hide()
    } else {
        $("#filesChooser").hide()
        $("#upload").hide()

    }
})
