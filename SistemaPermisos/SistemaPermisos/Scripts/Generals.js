
var ErrorAlGuardar = "Ocurrió un error al guardar";
var GuardadoCorrectamente = "Se guardó correctamente";
var SeEliminoCorrectamente = "Se eliminó correctamente";
var ErrorAlEliminar = "Ocurrió un error al eliminar";




var listar = function (url, arrayColumnas, parameters) {

    $.ajax({
        method: "GET",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (data) {

            window.Table(arrayColumnas, data);
            
        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}







function Table(arrayColumnas, data) {

    var container = document.getElementById("container");;
    container.classList.add("container");
    var contenido = "";

    contenido += "<input type='button' id='BtnAdd' value='Add' class='btn btn-success' onclick='return AbrirModal(true);'/>";

    if (data == null) {
        container.innerHTML = contenido;
    }

    contenido += "<table class='table mt-5'>";
    contenido += "<thead class='bg-dark text-white'>";
    contenido += "<tr>";

    for (i = 0; i < arrayColumnas.length; i++) {
        contenido += "<th>";
        contenido += arrayColumnas[i];
        contenido += "</th>";
    }

    contenido += "<th class='text-center'> Acciones </th>" ;

    contenido += "</tr>";
    contenido += "</thead>";
 
  
    var id;

    contenido += "<tbody>";
    for (row = 0; row < data.length; row++) {

        contenido += "<tr>";

        for (celda = 0; celda < arrayColumnas.length; celda++) {

            var cell = arrayColumnas[celda];

       
            contenido += "<td>";
            contenido += data[row][cell];
            contenido += "</td>";


            /*DE MANERA PERRONA OBTENEMOS EL ID PARA FILTRAR*/
            if (celda == 0) {
                id = data[row][cell];
            }
       
        }



 
        contenido += "<td class='text-center'>";
        contenido += "<button id='BtnEditar' class='editar btn btn-info btn-sm  style='height:30px' onclick='RecuperarInfo("+id+");'>  <i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>";
        contenido += "<button class='eliminar btn btn-danger btn-sm ml-5' style='height:30px' onclick='EliminarInfo(" + id +");'> <i class='fa fa-trash' aria-hidden='true'></i></button>";
        contenido += "</td>";


        contenido += "</tr>";

    }

    contenido += "</tbody>";
    contenido += "</table>";

    container.innerHTML = contenido;


}




var add = function (urlAdd, parameters, arrayColumnas) {


    $.ajax({
        method: "POST",
        url: urlAdd,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",

        success: function (data) {

            if (data != null) {
                alert(GuardadoCorrectamente);
                Table(arrayColumnas, data);
                Limpiar();
                CerrarModal();

            } else {
                alert(ErrorAlGuardar);
            }

     
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);
            alert(err.Message);
        }
    });

}



function AbrirModal(operacion) {


    if (operacion == true) {
        Limpiar();
        $(".modal-title").text("Agregar " + $("#Title").text());
        $("#modal").modal("show");
        operacion = null;
    }

    if (operacion == null) {
        $(".modal-title").text("Editar " + $("#Title").text());
        $("#modal").modal("show");
    }


  
}

function CerrarModal() {
    $("#modal").modal("hide");
}

function Limpiar() {
    $('#form').trigger("reset");
}



var filter = function (url, parameters, llenarModal, llenarTable, campos, selectedCombos) {

    $.ajax({
        method: "POST",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",


        success: function (data) {

            if (data != null && llenarModal == true) {
                LlenarModal(data, campos);

                if (selectedCombos == true) {
                    LlenarCombos(null, data);
                }

                AbrirModal();
            }

            if (data != null && llenarTable == true) {
                Table(arrayColumnas, data);
            }
        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}


function LlenarModal(data, campos) {

    //obtenemos los keys del JSON ejemplo: ID, NOMBRE...
    var keys = Object.keys(data[0]);

    for (row = 0; row < data.length; row++) {

        //recorremos los campos que vamos a pintar en el modal
        for (columna = 0; columna < campos.length; columna++) {

            //recorremos las keys
            for (o = 0; o < keys.length; o++) {

                if (keys[o] == campos[columna]) {

                    //celda actual
                    var celda = campos[columna];
                   
                    //obtenemos el valor de la celda
                    var value = data[row][celda];



                    //agregamos el valor obtenido al control mediante jquery el nombre del ID debe ser igual que la celda
                    $("#" + celda).val(value);
                    
          
                }//end if

            }//end for keys

        }// end for columna

    }

}


var LoadCombos= function (url) {


    $.ajax({
        method: "POST",
        url: url,
        dataType: "json",


        success: function (data) {

            LlenarCombos(data, null);

        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}


function LlenarCombos(data, dataSelected) {

    //para llenar DropDown
    var array = new Array();
    var idDropdown;
    var dropDown;

    var contenido = "";
    contenido += "<option value=" + "" + " >--CHOOSE--</option>";


    var selected;
    if (dataSelected != null) {
        data = null;
        data = dataSelected;
        selected = true;
    } else {
        selected = false;
    }


    var keys = Object.keys(data[0]);

    for (row = 0; row < data.length; row++) {


        //FILL DROPDOWNS

        for (k = 0; k < keys.length; k++) {

            var keyName = keys[k];

            var value = data[row][keyName];

            
            if (selected == true) {
                if (keyName.includes("VALUE")) {
                    var id = $("#" + keyName).val(value);
                }
            }

            if (selected == false) {

                if (keyName.includes("VALUE")) {
                    array[0] = value;
                    idDropdown = keyName;
                }
                if (keyName.includes("DISPLAY")) {
                    array[1] = value;
                }

                if (array.length != null && array.length == 2) {

                    contenido += "<option value=" + array[0] + ">" + array[1] + "</option>";
                    dropDown = document.getElementById(idDropdown);
                    dropDown.innerHTML = contenido;
                    idDropdown = "";
                    array = new Array();

                }

            }//end else

        }//end for keys dropdown

    }
}



function Delete(urlDelete, id, arrayColumnas) {


    if (confirm("Desea eliminar el registro con ID: " + id)) {

        var parametros = {
            "id": id,
        };

        $.ajax({
            method: "POST",
            url: urlDelete,
            data: parametros, /*parámetros enviados al controlador*/      
            dataType: "json",

            success: function (data) {
                datos = data;

                if (data != null) {

                    alert(SeEliminoCorrectamente);
                    Table(arrayColumnas, data);
                    Limpiar();
                    CerrarModal();


                } else {
                    alert(ErrorAlEliminar);
                }

              


            },
            error: function (xhr, status, error) {
                var err = JSON.parse(xhr.responseText);
                alert(err.Message);
            }
        });

    }

}


