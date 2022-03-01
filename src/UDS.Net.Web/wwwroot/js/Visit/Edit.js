// This code has potential issues becuase the value for complete is hard-coded

$(document).ready(function () {
    // initialize

    var checklistExists = $("form").data("checklistExists");
    var checklistStatus = $("form").data("checklistStatus");

    if (checklistExists) {
        var statusSelectBox = $("select[name='Status']");
        // Status can be changed to anything, but Complete if checklist is not completed (the D1 is completed during consensus)

        if (checklistStatus != "Complete") {
            statusSelectBox.find("option[value = '4']").prop("disabled", true);
        }
        else {
            $("input[name='IsSubmittedToNACC']").prop("disabled", false);
        }

        statusSelectBox.prop("disabled", false).removeClass("form-control-plaintext").addClass("form-control");
        var currentlySelectedStatus = $("select[name='Status'] option:selected");

        if (currentlySelectedStatus.text() == "Complete") {
            $("input[name='IsSubmittedToNACC']").prop("disabled", false);
        }
    }

    $("select[name='Status']").change(function () {
        console.log($(this).val());
        if ($(this).val() == 4) {
            $("input[name='IsSubmittedToNACC']").prop("disabled", false);
        }
        else {
            $("input[name='IsSubmittedToNACC']").prop("disabled", true);
        }
    });
});