




document.addEventListener("DOMContentLoaded", function (event) {
    window.listar("/Usuarios/List", ["ID", "NOMBRE"]);
});


function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());

    window.add("/Usuarios/Add", form, ["ID", "NOMBRE"]);

}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Usuarios/Filter", form, true, false, ["ID", "NOMBRE", "SELECT_VALUE_ROL", "SELECT_NOMBRE_ROL"]);
}


function EliminarInfo(id) {

    window.Delete("/Usuarios/Delete", id, ["ID", "NOMBRE"]);

}