// Onload fuctions
$(document).ready(function () {
    if ($('body').is('.InventoryLoad')) {
        if ($('#empty').val() == 'empty') {
            $input = $('<input type="file" name="PathFile" class="input-file" id="PathFile" />')
            $input.appendTo($('#form'))
            $file = $('<label for="PathFile" class="file">Select a file</label>')
            $file.appendTo($('#form'))
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        } else {
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Descargar Inventario" />')
            $button.appendTo($('#form'))
        }
    }
})
