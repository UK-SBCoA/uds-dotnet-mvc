function reviewCheckboxesStates() {
    var countChecked = $("input[type=checkbox]:checked").length;
    if (countChecked > 0) {
        $("input[name=CurrentlyTakingMedicationsControl][value='true']").prop("disabled", false);
        $("input[name=CurrentlyTakingMedicationsControl][value='true']").prop("checked", true);
        $("input[name=CurrentlyTakingMedicationsControl][value='false']").prop("disabled", true);
        $("input[name=CurrentlyTakingMedications]").val(true);
    }
    else {
        $("input[name=CurrentlyTakingMedicationsControl][value='false']").prop("disabled", false);
        $("input[name=CurrentlyTakingMedicationsControl][value='false']").prop("checked", true);
        $("input[name=CurrentlyTakingMedicationsControl][value='true']").prop("disabled", true);
        $("input[name=CurrentlyTakingMedications]").val(false);
    }
}

jQuery(() => {
    // set the default state from the server
    var currentlyTakingMedications = $("input[name=CurrentlyTakingMedications]");
    if (currentlyTakingMedications != null && currentlyTakingMedications.val() != null) {
        if (currentlyTakingMedications.val() == "True") {
            $("input[name=CurrentlyTakingMedicationsControl][value='true']").prop("checked", true);
        }
        else if (currentlyTakingMedications.val() == "False") {
            $("input[name=CurrentlyTakingMedicationsControl][value='false']").prop("checked", true);
        }
    }

    // check the current status of checkboxes

    reviewCheckboxesStates();
});


$("input[name=CurrentlyTakingMedicationsControl]").on("change", function () {
    var currentlyTakingMedicationsValue = $(this).val();
    $("input[name=CurrentlyTakingMedications]").val(currentlyTakingMedicationsValue);
});

function submitCurrent(element) {
    var drugId = $(element).attr('id');
    $("#loading_" + drugId).show();
    $(element).hide();
    var form = $("#form_" + drugId);
    if (form) {
        var token = form.find('input[name="__RequestVerificationToken"]').val();
        var formData = form.serialize();
        $.ajax({
            type: element.form.method,
            url: element.form.action,
            data: formData,
            headers: { "RequestVerificationToken": token },
            success: function (response) {
                $("#loading_" + drugId).hide();
                $(element).show();

                var medicationCurrentId = response.medicationCurrent.id;
                if ($(element).prop("checked") == true) {
                    $("#" + drugId + "_MedicationCurrent_Id").val(medicationCurrentId);
                }
                else {
                    $("#" + drugId + "_MedicationCurrent_Id").val("");
                }
                reviewCheckboxesStates();
            },
            error: function (response) {
                $("#loading_" + drugId).hide();
                $(element).show();
            }
        });
    }

}