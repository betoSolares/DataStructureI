﻿// Onload fuctions
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
$('.product').on('click', function (e) {
    if (e.target == this) {
        name = $(this).find('span:first').text()
        GetData(name)
    }
})

// Get the information from the server
function GetData(param) {
    $.get('/Inventory/ProductInfo', { name: param }, function (data) {
        $('#productInfo').show()
        if ($('#alertBox').show()) {
            $('#alertBox').hide()
        }
        if (data.name != 'null') {
            $('#name').text("Nombre: " + data.name)
            $('#description').text("Descripción: " + data.description)
            $('#production').text("Productor: " + data.production)
            $('#price').text("Precio: $" + data.price)
            $('#quantity').attr({ "max": data.stock })
        } else {
            $('#alertBox').show()
            $('#alertBox').attr('class', 'failed')
            $('#message').text('El producto ya no existe. Toca en cualquier parte de la pantalla para salir.')
            $('.info').hide()
        }
    })
}

// Download the product state
$('#download').click(function () {
    text = $('#name').text()
    name = text.slice(7, text.length).trim()
    location.href = '/Inventory/DownloadProduct/?file=' + name
})

// Add the product to the shop cart
$('#addCart').click(function () {
    text = $('#name').text()
    name = text.slice(7, text.length).trim()
    quantity = parseInt($('#quantity').val(), 10)
    maxValue = parseInt($('#quantity').attr('max'), 10)
    if (quantity > 0 && quantity <= maxValue) {
        $.post('/Inventory/AddProductCart', { name: name, quantity: quantity }, function (data) {
            $('#quantity').attr({ "max": data.max })
            CartMessage('success')
        })
    } else {
        CartMessage('failed')
    }
})

function CartMessage(state) {
    if (state == 'success') {
        $('#alertBox').show()
        $('#alertBox').attr('class', 'success')
        $('#message').text('El producto se agrego con exito al carrito de compras.')
    } else if (state == "failed") {
        $('#alertBox').show()
        $('#alertBox').attr('class', 'failed')
        $('#message').text('Hubo un error. Verifica que la cantidad de productos sea la correcta.')
    }
}

// Messages for the remove product action
function RemoveMessages(state) {
    if (state == 'success') {
        $('#alertBox').show()
        $('#alertBox').attr('class', 'success')
        $('#message').text('El producto se elimino de su carrito de compras.')
    } else if (state == "failed") {
        $('#alertBox').show()
        $('#alertBox').attr('class', 'failed')
        $('#message').text('Hubo un error. Verifica que la cantidad de productos sea la correcta.')
    }
}

// Partially remove the product
$('.removePartial').on('click', function () {
    quantity = parseInt($('#numberRemove').val(), 10)
    maxValue = parseInt($('#numberRemove').attr('max'), 10)
    if (quantity > 0 && quantity <= maxValue) {
        name = $(this).parent().find('span:first').text()
        $.post('/Inventory/PartialRemove', { name: name , quantity: quantity }, function (data) {
            if (data) {
                location.reload()
            } else {
                RemoveMessages("failed")
            }
        })
    } else {
        RemoveMessages("failed")
    }
})


$('.removeComplete').on('click', function () {
    name = $(this).parent().find('span:first').text()
    $.post('/Inventory/CompleteRemove', { name: name }, function (data) {
        if (data) {
            location.reload()
        } else {
            RemoveMessages("failed")
        }
    })
})
