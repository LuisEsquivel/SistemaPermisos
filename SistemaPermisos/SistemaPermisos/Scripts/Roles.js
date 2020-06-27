

document.addEventListener("DOMContentLoaded", function (event) {
    window.listar("/Roles/List", ["ID", "NOMBRE", "FECHA_ALTA"], null, "/Roles/LoadCheckBox");
    
});


function Add() {

    var form = new FormData();

    form.append("ID", $("#ID").val());
    form.append("NOMBRE", $("#NOMBRE").val());

    form.append("checkbox", checkbox());
    
    window.add("/Roles/Add", form, ["ID", "NOMBRE", "FECHA_ALTA"]);
   
}


function RecuperarInfo(id) {
    var form = new FormData();

    form.append("ID", id);

    window.filter("/Roles/Filter", form, true, false, ["ID", "NOMBRE", "FECHA_ALTA"], false, "/Roles/LoadCheckBox");
}


function EliminarInfo(id) {

    window.Delete("/Roles/Delete", id, ["ID", "NOMBRE", "FECHA_ALTA"]);

}


function checkbox() {
    var ncheckbox = document.getElementsByClassName("chk");
    var checkbox = new Int32Array(ncheckbox.length);

    for (i = 0; i < ncheckbox.length; i++) {
        if (ncheckbox[i].checked == true) {
            checkbox[i] = ncheckbox[i].id;
        }

    }

    var checks;
    if (checkbox.length > 0) {
        checks = checkbox;
    }

    return checks;
}