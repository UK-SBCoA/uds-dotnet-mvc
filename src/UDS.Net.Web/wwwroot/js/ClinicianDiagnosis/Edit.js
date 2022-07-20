
// Clinician Diagnosis Edit form

$(document).ready(function () {

    // For other questions with children that only have 1 level of dependency

    function syndromeWithChildren(isChecked, childElements) {
        var disabled = !isChecked;

        $.each(childElements, function (index, value) {
            $('input[name="' + value + '"]').prop("disabled", disabled);
            console.log(value + " disabled to " + disabled);
            if (!isChecked) {
                $('input[name="' + value + '"]').prop('checked', false); // only unselect on not checked
            }
        });
    }

    // Section 3: Etiologic diagnoses

    function etiologyState(present, childElements) {
        // indicate all present
        // for subjects with normal cognition, indicate presence, but leave diagnosis blank
        // for subjects with impaired cognition, only indicate one primary

        var disabled = !present; // toggle
        var uncheck = false;

        if ($("input[name=HasNormalCognition]:checked").val() === "true") {
            disabled = true;
            uncheck = true;
        }

        $.each(childElements, function (index, value) {
            $('input[name="' + value + '"]').prop("disabled", disabled);

            if (!present || uncheck) {
                console.log("unchecking " + value); // TODO remove debug
                $('input[name="' + value + '"]').prop('checked', false); // only unselect on not checked
            }
        });
    }

    function etiologyStateForNorm(present, normalChildElements) {
        // this needs to run after the etiologyState when normal

        var disabled = !present;

        $.each(normalChildElements, function (index, value) {
            $('input[name="' + value + '"]').prop("disabled", disabled);

            if (!present) {
                console.log("unchecking " + value); // TODO remove debu
                $('input[name="' + value + '"]').prop('checked', false); // only unselect on not checked
            }
        });
    }

    // Question 5
    // Send the checkbox that is currently selected and it will unselect all
    // other checkbox children radio and disable

    function oneMCISyndromeAllowed(elementName) {
        $('.oneMCISyndrome:not(input[name="' + elementName + '"])').each(function () {
            // uncheck self

            $(this).prop("checked", false);

            // unselect children

            var childElements = $(this).data("children");
            syndromeWithChildren(false, childElements);

        });
    }


    // Question 3
    // Conditional states for dementia 
    // if false then disable Question #4
    // if true then disable Question #5

    function hasDementiaState(hasDementia) {

        var toggle = false; // jquery prop needs a bool
        if (hasDementia === "true") {
            toggle = true;
        } 

        if (hasDementia != null) { // if Question 2 is no

            $(".meetsCriteriaForDementia").prop("disabled", !toggle); // question 4
            $(".notDementia").prop("disabled", toggle); // question 5

            if (hasDementia === "true") {
                // Show Question 4 (.meetsCriteriaForDementia)

                // Uncheck and disable Question 5

                $('.oneMCISyndrome:checked').each(function () {
                    // uncheck

                    $(this).prop("checked", false);

                    // uncheck and disable children

                    var childElements = $(this).data("children");
                    syndromeWithChildren(false, childElements);

                });

            } else {
                // Show question 5 (.notDementia)

                $(".oneMCISyndrome").each(function () {
                    var currentName = $(this).prop("name");
                    var checkedState = $(this).is(":checked");

                    var childElements = $(this).data("children");
                    syndromeWithChildren(checkedState, childElements);

                });

                // uncheck and disabled Question 4

                $(".meetsCriteriaForDementia").prop("checked", false);
                

            }

        } else {
            // If Question 2 is yes

            $(".meetsCriteriaForDementia").prop("disabled", true);
            $(".meetsCriteriaForDementia").prop("checked", false);

            $(".notDementia").prop("disabled", true);
            $(".notDementia").prop("checked", false);

            $(".oneMCISyndrome").each(function () {
                var currentName = $(this).prop("name");

                var childElements = $(this).data("children");
                syndromeWithChildren(false, childElements);
            });

        }

        $(".syndrome").each(function () {

            var syndrome = $(this).prop("name");
            var disabled = $(this).prop("disabled");
            var present = $(this).is(":checked");
            var childElements = $(this).data("children");

            syndromeWithChildren(present, childElements);
        });
    }

    // Question 2
    // if true then normal cognition then disable Question #3, #4, #5
    // if false then impaired

    function hasNormalCognitionState(hasNormalCognition) {
        
        var disabled = true;
        if (hasNormalCognition === "false") {
            disabled = false; // if false then enable #3
        }

        // toggle disabled on all children

        $(".hasNormalCognitionToggle").prop('disabled', disabled);

        // now toggle their child question states according to their logic

        if (hasNormalCognition === "true") {
            hasDementiaState(null);
        } else if (hasNormalCognition === "false") {
            // Question 3 Dementia State, children: #4 and #5
            var meetsCriteriaForDementia = $("input[name=MeetsCriteriaForDementia]:checked");
            if (typeof meetsCriteriaForDementia !== "undefined") {
                hasDementiaState(meetsCriteriaForDementia.val());
            }
        } else {
            hasDementiaState(null);
        }

        // Section 3 diagnoses requirement is affected by hasNormalCognition

        $(".etiology").each(function () {
            var etiology = $(this).prop("name");
            var present = $(this).is(":checked");
            var childElements = $(this).data("children");
            
            etiologyState(present, childElements);

            if (hasNormalCognition === "true") {
                var normalChildElements = $(this).data("normal");
                if (normalChildElements) {
                    etiologyStateForNorm(present, normalChildElements);
                }
            }

        });

    }




    // initialize

    var hasNormalCognition = $('input[name="HasNormalCognition"]');
    if (hasNormalCognition.is(':checked')) {
        var whichCognitionChecked = $('input[name="HasNormalCognition"]:checked'); // radio button groups are interesting
        hasNormalCognitionState(whichCognitionChecked.val());
    } else {
        hasNormalCognitionState(null);
    }

    $(".syndrome").each(function () {

        var syndrome = $(this).prop("name");

        var disabled = $(this).prop("disabled");

        var present = $(this).is(":checked");
        var childElements = $(this).data("children");

        syndromeWithChildren(present, childElements);

    });

    $(".syndromeGroup").each(function () {
        var syndrome = $(this).prop("name");
        var present = $(this).is(":checked");
        if (present) {
            var childElements = $(this).data("children");
            var childState = $(this).data("childstate");
            console.log("syndrome group " + syndrome + " " + present);
            syndromeWithChildren(childState, childElements);
        }
    });

    $(".frontotemporalLobarDegeneration").each(function () {
        if ($(this).is(":checked")) {
            syndromeWithChildren(true, ["FTLDSubtype"]);
        }
    });

    // EVENTS

    // Question 2 Has Normal Cognition

    $("input[name=HasNormalCognition]").on("change", function () {
        var currentValue = $(this).val();
        hasNormalCognitionState(currentValue);
    });

    // Question 3 Has dementia

    $("input[name=MeetsCriteriaForDementia]").on("change", function () {
        var dementiaValue = $(this).val();
        hasDementiaState(dementiaValue);
    });

    // Question 5 MCI syndrome and affected domains

    $(".oneMCISyndrome").on("change", function () {
        var currentValue = $(this).is(":checked");
        var currentName = $(this).prop("name");
        oneMCISyndromeAllowed(currentName);
    });


    $(".syndrome").on("change", function () {
        var syndrome = $(this).prop("name");
        var present = $(this).is(":checked");
        var childElements = $(this).data("children");

        syndromeWithChildren(present, childElements);
    });

    $(".syndromeGroup").on("change", function () {
        var syndrome = $(this).prop("name");
        var present = $(this).is(":checked");
        var childElements = $(this).data("children");
        var childState = $(this).data("childstate");

        syndromeWithChildren(childState, childElements);
    });

    $(".etiology").on("change", function () {
        var etiology = $(this).prop("name");
        var present = $(this).is(":checked");
        var childElements = $(this).data("children");

        // only toggle if cognition is not marked as normal
        var hasNormalCognition = $("input[name=HasNormalCognition]:checked");
        if (typeof hasNormalCognition !== "undefined" && hasNormalCognition.val() != 'true') {
            etiologyState(present, childElements);
        }
        else {
            var normalChildElements = $(this).data("normal");
            // TODO remove debug
            console.log(normalChildElements);
            etiologyStateForNorm(present, normalChildElements);
        }

    });

    // Question 14 is a special case

    $(".frontotemporalLobarDegeneration").on("change", function () {
        // if one or more are checked then enable FTLD subtype

        var atLeastOneChecked = false;
        var counter = 0;
        $(".frontotemporalLobarDegeneration").each(function () {
            if ($(this).is(":checked")) {
                counter++;
            }
        });

        if (counter > 0) {
            atLeastOneChecked = true;
        }
        syndromeWithChildren(atLeastOneChecked, ["FTLDSubtype"]);

    });

});

