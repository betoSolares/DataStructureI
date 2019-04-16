﻿// Hide or show the menu in mobile display
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

// Hide the unused elements in the views
$(document).ready(function () {
    if ($("#dictionaries").val() == "empty") {
        if ($("body").hasClass("LoadFiles")) {
            $("#download").hide()
        } else if ($("body").hasClass("SearchSticker")) {
            $("#searchbox").hide()
        }
    } else {
        if ($("body").hasClass("LoadFiles")) {
            $("#filesChooser").hide()
            $("#upload").hide()
        } else if ($("body").hasClass("SearchSticker")) {
            $("#noDictionarie").hide()
        }
    }
})

// Make the notification appear in the LoadFiles view
$(document).ready(function () {
    notification = $(".notification")
    if ($("#state").val() == "success") {
        notification.show()
        notification.attr("class", "notification is-success")
        $("#message").text("Los archivos se han leido y cargado de forma exitosa.")
    } else if ($("#state").val() == "failed") {
        notification.show()
        notification.attr("class", "notification is-danger")
        $("#message").text("Hubo un error a la hora de cargar los archivos. Verifique que loas archivos sean los correctos")
    }
})

// Delete button hide parent container
$(".delete").click(function () {
    $(".delete").parent().hide()
})

// Disable the search field if the special radio button is checked
$(".radio").on("change", function () {
    if ($(this).children().is("#specialRadio")) {
        $("#search").prop("disabled", true);
    } else {
        $("#search").prop("disabled", false);
    }
})