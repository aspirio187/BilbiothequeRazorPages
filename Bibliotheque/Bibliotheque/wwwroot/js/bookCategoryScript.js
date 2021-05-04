jQuery(document).ready(loadCategories());

function loadCategories() {
    $.ajax({
        url: '/api/Category/GetCategories',
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            console.log(response);
            $("#categoriesOptions").empty();
            $("#categoriesOptions").append("<option value='0'></option>");

            for (var i = 0; i < response.length; i++) {
                let id = response[i]['id'];
                let name = response[i]['name'];
                $("#categoriesOptions").append("<option value='" + id + "'>" + name + "</option>");
            }
        },
        error: function (xhr) {
            $.notify(xhr.responseText, "error");
            console.log(xhr);
        }
    })
}

function submitCategoryForm() {
    let requestData = {
        Id: parseInt($("#categoryId").val()),
        Name: $("#categoryName").val()
    }

    $.ajax({
        url: '/api/category/addcategory',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (response) {
            loadCategories();
            closeModal();
            $.notify("La catégorie " + response.name + " a bien été ajoutée", "success");
            $("#categoryNameValidation").text('');
        },
        error: function (xhr) {
            if (xhr.responseJSON === undefined) {
                $.notify(xhr.responseText, "error");
            } else {
                if (xhr.responseJSON.status === 400) {
                    let errors = xhr.responseJSON.errors.Name;
                    $("#categoryNameValidation").text(errors[0]);
                }
            }

        }
    });
}

function deleteCategory() {
    let id = parseInt($("#categoryId").val());
    $.ajax({
        url: '/api/category/deletecategory/' + id,
        type: 'DELETE',
        dataType: 'JSON',
        success: function (response) {
            $.notify("La catégorie a bien été supprimée", "success");
            closeModal();
            loadCategories();
            console.log(response);
        },
        error: function (xhr) {
            console.log(xhr);
            $.notify(xhr.responseText, "error");
        }
    });
}

function showModal(obj) {
    if (obj === null || obj === undefined) {
        $("#categoryId").val($("#categoriesOptions").val());
        $("#categoryName").val($("#categoriesOptions option:selected").text());
    } else {
        $("#categoryId").val(obj.id);
        $("#categoryName").val(obj.name);
    }

    if (categoryIsEmpty()) {
        $("#btnSubmitCategory").html("Ajouter");
        $("#btnDeleteCategory").addClass("d-none");
    } else {
        $("#btnSubmitCategory").html("Modifier");
        $("#btnDeleteCategory").removeClass("d-none");
    }
    $("#categoryInput").modal("show");
}

function closeModal() {
    $("categoryId").val('');
    $("#categoryName").val('');
    $("#categoryInput").modal("hide");
}

function categoryIsEmpty() {
    let id = $("#categoryId").val();
    if (id !== "0" && id !== undefined) {
        return false;
    }
    let name = $("#categoryName").val();
    if (name.length > 0 && name !== "" && name !== undefined) {
        return false;
    }
    return true;
}