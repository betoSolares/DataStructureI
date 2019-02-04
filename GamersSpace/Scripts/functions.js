// Find game information
function GameInfo(gameList) {
    inputValue = document.getElementById('inputText').value;
    if (!isNaN(inputValue)) {
        if (inputValue <= gameList.length) {
            alert(gameList[inputValue - 1].name)
        }
        else {
            alert("No existe ese valor.")
        }
    }
    else {
        alert("Por favor inserte un numero.")
    }    
}
