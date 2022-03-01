$(document).ready(function () {
    
    $("#edit-form").on("click", function () {
        $("#edit-form").addClass("d-none");
        $("#submit").removeClass("d-none");
        $("#cancel").removeClass("d-none");

        $("select[name='CoordinatorInitials']").prop("disabled", false).removeClass("form-control-plaintext").addClass("form-control");
        $("select[name='ClinicianInitials']").prop("disabled", false).removeClass("form-control-plaintext").addClass("form-control");
        $("select[name='SocialWorkerInitials']").prop("disabled", false).removeClass("form-control-plaintext").addClass("form-control");

        var checklistExists = $("form").data("checklistExists");
        var checklistStatus = $("form").data("checklistStatus");
        // the checklist is the minimum required for the overall packet to be able to be submitted, so it drives a couple things
        // 1. If the packet can be completed
        // 2. If the packet can be marked as submitted to NACC

        if (checklistExists)
        {
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

            console.log(currentlySelectedStatus.text());
            if (currentlySelectedStatus.text() == "Complete") {
                $("input[name='IsSubmittedToNACC']").prop("disabled", false);
            }
        }

    });

    $("#cancel").on("click", function () {
        $("#edit-form").removeClass("d-none");
        $("#submit").addClass("d-none");
        $("#cancel").addClass("d-none");

        $("select[name='CoordinatorInitials']").prop("disabled", true).addClass("form-control-plaintext").removeClass("form-control");
        $("select[name='ClinicianInitials']").prop("disabled", true).addClass("form-control-plaintext").removeClass("form-control");
        $("select[name='SocialWorkerInitials']").prop("disabled", true).addClass("form-control-plaintext").removeClass("form-control");
        $("select[name='Status']").prop("disabled", true).addClass("form-control-plaintext").removeClass("form-control");
        $("input[name='IsSubmittedToNACC']").prop("disabled", true);

    });

    $("select[name='Status']").change(function () {
        if ($(this).val() == 4) {
            $("input[name='IsSubmittedToNACC']").prop("disabled", false);
        }
        else {
            $("input[name='IsSubmittedToNACC']").prop("disabled", true);
        }
    });
});