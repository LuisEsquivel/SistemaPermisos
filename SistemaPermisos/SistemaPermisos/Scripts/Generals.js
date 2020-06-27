
var ErrorAlGuardar = "Ocurrió un error al guardar";
var GuardadoCorrectamente = "Se guardó correctamente";
var SeEliminoCorrectamente = "Se eliminó correctamente";
var ErrorAlEliminar = "Ocurrió un error al eliminar";
var ElRegistroYaExiste = "El Registro Ya Existe!!" + "\n" + "Verifique";
var idTable = "";



var listar = function (url, arrayColumnas, parameters, urlLoadCheckBox) {

    $.ajax({
        method: "GET",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (data) {

            Table(arrayColumnas, data);
            LoadCheckbox(urlLoadCheckBox);
            
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

    contenido += "<input type='button' id='BtnAdd' value='Add' class='btn btn-success' onclick='return AbrirModal(1);'/>";

    if (data == null) {
        container.innerHTML = contenido;
    }

    idTable = "Table" + $("#Title").text();

    contenido += "<table class='table mt-5' id='" + idTable+"'>";
    contenido += "<thead class='bg-dark text-white'>";
    contenido += "<tr>";

    for (i = 0; i < arrayColumnas.length; i++) {
        contenido += "<th>";

        var theadColumn = arrayColumnas[i];

        if (theadColumn.includes("_")) {
            theadColumn = theadColumn.replace("_", " ");
        }
        contenido += theadColumn;
        contenido += "</th>";
    }

    contenido += "<th class='text-center'> ACCIONES </th>" ;

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
        contenido += "<button class='eliminar btn btn-danger btn-sm ml-2' style='height:30px' onclick='EliminarInfo(" + id +");'> <i class='fa fa-trash' aria-hidden='true'></i></button>";
        contenido += "</td>";


        contenido += "</tr>";

    }

    contenido += "</tbody>";
    contenido += "</table>";

    container.innerHTML = contenido;

    $('#' + idTable).DataTable({
        lengthMenu: [
            [7, 14, 21, -1],
            ['7 rows', '14 rows', '21 rows', 'Todo']
        ],
 
        order: [[0, "desc"]],

        language: Language
    });


    TableDesign();
}

function TableDesign() {
    $("#" + idTable + "_filter").addClass("text-primary");
    $("#" + idTable + "_length").addClass("text-primary");
    $("#" + idTable + "_info").addClass("text-primary");
    $("#" + idTable + "_paginate").addClass("text-primary");
    $(".form-control-sm").addClass("text-primary");

}

$(document).ready(function () {

    $.extend($.fn.dataTableExt.oStdClasses, {
        "sFilterInput": "form-control text-primary",
        "sLengthSelect": "form-control dropdown mr-2 ml-2 pr-1  pb-1 pt-1  text-primary"
    });

});




var add = function (urlAdd, parameters, arrayColumnas, checkbox) {


    $.ajax({
        method: "POST",
        url: urlAdd,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",

        success: function (data) {


            if (data.success == true) {
                alert(ElRegistroYaExiste);
            } else if (JSON.stringify(data).includes(":(")) {
                alert("El registro con los valores: " +"\n"+  data.exist + "\n" + "Ya Existe" +"\n"+ "Verifique" )
            }
            else {


                if (data != null) {
                    alert(GuardadoCorrectamente);
                    Table(arrayColumnas, data);
                    Limpiar();
                    CerrarModal();

                } else {
                    alert(ErrorAlGuardar);
                }

            }
     
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);
            alert(err.Message);
        }
    });

}





var filter = function (url, parameters, llenarModal, llenarTable, campos, selectedCombos, urlLoadCheckBox) {

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

        
            if (urlLoadCheckBox != null) {
            LoadCheckbox(urlLoadCheckBox, parameters.get("ID"));

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



var LoadCheckbox = function (url, id = 0) {


    if (url != null) {

        var parametros = {
            "id": id,
        };

        $.ajax({
            method: "POST",
            url: url,
            data: parametros,
            dataType: "json",
   

            success: function (data) {

                PintarCheckBox(data, id);

            },
            error: function () {
                console.log("No se ha podido obtener la información");
            }
        });

    }

}


function PintarCheckBox(data, idChecked = 0) {

    if (data != null) {

        var ContainerCheckBox = document.getElementById("ContainerCheckBox");
        var contenido = "";
        contenido += "<h4 class='mt-5'>Operaciones</h4>"

        var keys = Object.keys(data[0]);


        for (i = 0; i < data.length; i++) {

            var CheckBoxName = "";
            var CheckBoxValue = 0;
            var Ckecked = 0;
            var id = 0;


            for (k = 0; k < keys.length; k++) {

                var name = keys[k];
      
                if (name == "ID") {
                    CheckBoxValue  = data[i][name];
                    id = data[i][name];
                } 

                if (name == "NOMBRE") {
                    CheckBoxName = data[i][name];
                }

                if (name.includes("CHECKED")) {
                    Ckecked = data[i][name];
                }

            }

            //contenido += "<div class='form-check ml-4 mt-3'>";

            contenido += "<label>";

         
            if (Ckecked > 0 && Ckecked == idChecked) {
                contenido += "<input type='checkbox' class='chk' checked  value='" + CheckBoxValue + "' id='" + id +"'>";

            }

            if (Ckecked == 0 || Ckecked != idChecked) {
                contenido += "<input type='checkbox' class='chk'  value='" + CheckBoxValue + "' id='" + id + "'>";

            }

          
            contenido += CheckBoxName;
            contenido += "</label >";
            contenido += "</br >";

            //contenido += "</div>";
       

        }

     }


       ContainerCheckBox.innerHTML = contenido;

    }




function AbrirModal(operacion) {

    if (operacion == 1) {
        Limpiar();
        $(".modal-title").text("Agregar " + $("#Title").text());
        $("#modal").modal("show");
    }

    if (operacion == null) {
        $(".modal-title").text("Editar " + $("#Title").text());
        $("#modal").modal("show");
    }

    operacion = null;

}

function CerrarModal() {
    $("#modal").modal("hide");
}

function Limpiar() {

    //there are checkbox
    if ($(".chk").length > 0) {
        $(".chk").removeAttr("checked");
    }

    $('#form').trigger("reset");


}





Language =
    {

        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla =(",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        },
        "buttons": {
            "copy": "Copiar",
            "colvis": "Visibilidad"
        }

    }
