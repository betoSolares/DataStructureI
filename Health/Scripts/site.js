// Onload fuctions
$(document).ready(function () {
    if ($('body').is('.InventoryLoad')) {
        if ($('#empty').val() == 'empty') {
            $fiel = $('<input type="file" name="PathFile" class="input-file" id="fileChooser" />')
            $fiel.appendTo($('#form'))
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        } else {
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        }
    }
})