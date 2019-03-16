$(document).ready(function () {
    if ($('body').is('.InventoryLoad')) {
        if ($('#empty').val() == 'empty') {
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        } else {
            $button = $('<input type="submit" class="button-primary btn" name="formAction" value="Cargar Inventario" />')
            $button.appendTo($('#form'))
        }
    }
})