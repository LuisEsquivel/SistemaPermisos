

document.addEventListener("DOMContentLoaded", function (event) {
    window.LoadCombos("/Usuarios/FillCombos");
    window.listar("/Usuarios/List", ["ID", "NOMBRE"], null, true);
});


function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());
    form.append("ID_ROL", $("#VALUE_ROL").val());

    window.add("/Usuarios/Add", form, ["ID", "NOMBRE"]);
    
}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Usuarios/Filter", form, true, false, ["ID", "NOMBRE"], true, true);
    
}


function EliminarInfo(id) {

    window.Delete("/Usuarios/Delete", id, ["ID", "NOMBRE"]);

}


function SelectedCombos(id) {
    var form = new FormData();

    form.append("ID", id);

    window.LlenarCombos("/Usuarios/FillCombos");
}