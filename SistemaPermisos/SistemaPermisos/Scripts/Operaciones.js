



document.addEventListener("DOMContentLoaded", function (event) {
    //window.LoadCombos("/Operaciones/FillCombos");
    window.listar("/Operaciones/List", ["ID", "NOMBRE", "FECHA_ALTA"]);

});


function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());
    window.add("/Operaciones/Add", form, ["ID", "NOMBRE", "FECHA_ALTA"]);

}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Operaciones/Filter", form, true, false, ["ID", "NOMBRE", "FECHA_ALTA"], true);
}


function EliminarInfo(id) {

    window.Delete("/Operaciones/Delete", id, ["ID", "NOMBRE", "FECHA_ALTA"]);

}

function SelectedCombos() {
    window.LlenarCombos("/Operaciones/FillCombos");
}