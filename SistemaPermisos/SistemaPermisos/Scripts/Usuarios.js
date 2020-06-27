

document.addEventListener("DOMContentLoaded", function (event) {
    window.LoadCombos("/Usuarios/FillCombos");
    window.listar("/Usuarios/List", ["ID", "NOMBRE", "ROL", "FECHA_ALTA"], null, null);
});



function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());
    form.append("ID_ROL", $("#VALUE_ROL").val());

    window.add("/Usuarios/Add", form, ["ID", "NOMBRE", "ROL", "FECHA_ALTA"]);
    
}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Usuarios/Filter", form, true, false, ["ID", "NOMBRE", "ROL", "FECHA_ALTA"], true, null);
    
}


function EliminarInfo(id) {

    window.Delete("/Usuarios/Delete", id, ["ID", "NOMBRE","ROL", "FECHA_ALTA"]);

}


function SelectedCombos() {
    window.LlenarCombos("/Usuarios/FillCombos");
}