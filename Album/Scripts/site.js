// Make the search for the stickers
$("#buttonSearch").click(function () {
    if ($("input[name=type]:checked").val() == "Special") {
        $.post("/Album/SearchSticker", { type: "Special" }, function (data) {
            console.log(data)
        })
    } else {
        $.post("/Album/SearchSticker", { search: $("#search").val(), type: "Team" }, function (data) {
            console.log(data)
        })
    }
})