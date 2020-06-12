

document.addEventListener("DOMContentLoaded", function (event) {
    window.listar("/Roles/List", ["ID", "NOMBRE"]);
    
});


function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());
    window.add("/Roles/Add", form, ["ID", "NOMBRE"]);
   
}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Roles/Filter", form, true, false, ["ID", "NOMBRE"]);
}


function EliminarInfo(id) {

    window.Delete("/Roles/Delete", id, ["ID", "NOMBRE"]);

}