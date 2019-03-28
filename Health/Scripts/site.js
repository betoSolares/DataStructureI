// Onload fuctions
$(document).ready(function () {
    if ($('body').is('.InventoryLoad')) {
        if ($('#empty').val() == 'empty') {
            $input = $('<input type="file" name="PathFile" class="input-file" id="PathFile" accept=".csv" />')
            $input.appendTo($('#form'))
            $file = $('<label for="PathFile" class="file">Selecciona un archivo</label>')
            $file.appendTo($('#form'))
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        } else {
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Descargar Inventario" />')
            $button.appendTo($('#form'))
        }
    }
})

// Alert messages
$(document).ready(function () {
    if ($('body').is('.InventoryLoad')) {
        if ($('#state').val() == 'success') {
            $('#alertBox').show()
            $('#alertBox').addClass('success')
            $('#message').text('El archivo se cargo al arbol exitosamente.')
        } else if ($('#state').val() == "failed") {
            $('#alertBox').show()
            $('#alertBox').addClass('failed')
            $('#message').text('Ocurrio un error. Verifica que hayas seleccionado el archivo correcto.')
        }
    }
})

// Show product information
$('.product').on('click', function () {
    text = $(this).children().text()
    dashIndex = text.indexOf('-')
    name = text.substring(0, dashIndex).trim()
    GetData(name)
})

// Get the information from the server
function GetData(param) {
    $.get('/Inventory/ProductInfo', { name: param }, function (data) {
        $('#productInfo').show()
        $('#name').text("Nombre: " + data.name)
        $('#description').text("Descripción: " + data.description)
        $('#production').text("Productor: " + data.production)
        $('#price').text("Precio: $" + data.price)
        $('#quantity').attr({ "max" : data.stock })
    })
}
