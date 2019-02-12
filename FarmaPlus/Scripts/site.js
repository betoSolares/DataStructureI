// Style of the alert 
window.onload = function () {
    alertBox = document.getElementById("alertBox")
    state = document.getElementById("state").value
    message = document.getElementById("message")
    text = ""
    if (state == "success") {
        alertBox.className = "success"
        text = document.createTextNode("Se añadio el empleado de manera correcta.")
    }
    else if (state == "failed") {
        alertBox.className = "failed"
        text = document.createTextNode("Ya existe un empleado con el mismo codigo.")
    }
    else {
        alertBox.style.display = "none"
    }
    message.appendChild(text)
}