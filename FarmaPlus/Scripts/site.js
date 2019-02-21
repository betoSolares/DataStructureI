// Style of the alert 
window.onload = function () {
    alertBox = document.getElementById("alertBox")
    state = document.getElementById("state").value
    message = document.getElementById("message")
    text = ""
    if (state == "success") {
        alertBox.className = "success"
        text = document.createTextNode("Se añadio el empleado de manera correcta.")
    } else if (state == "failed") {
        alertBox.className = "failed"
        text = document.createTextNode("Ya existe un empleado con el mismo codigo.")
    } else if (state == "failedArrive") {
        alertBox.className = "failed"
        text = document.createTextNode("Por favor verifique si el empleado ya esta en la oficina o si existe.")
    } else if (state == "successArrive") {
        alertBox.className = "success"
        text = document.createTextNode("Se guardo el estado del empleado.")
    } else if (state == "failedSimulation") {
        alertBox.className = "failed"
        text = document.createTextNode("No hay empleados por salir.")
    } else {
        alertBox.style.display = "none"
    }
    message.appendChild(text)
}