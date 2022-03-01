// Milestones create and edit forms

$(document).ready(function () {

    function toggleBoxASection(state)
    {
        var disabledState = !state;
        $(".boxAToggle").prop("disabled", disabledState);
    }

    function toggleBoxBSection(state)
    {
        var disabledState = !state;
        $(".boxBToggle").prop("disabled", disabledState);
    }

    // initialize

    var boxSelected = $('input[name="MilestoneType"]');

    if (boxSelected.is(':checked')) {
        var whichBoxChecked = $('input[name="MilestoneType"]:checked');
        console.log(whichBoxChecked.val());
        if (whichBoxChecked.val() == "A") {
            toggleBoxASection(true);
            toggleBoxBSection(false);
        }
        else {
            toggleBoxASection(false);
            toggleBoxBSection(true);
        }
    }

    // event handlers

    boxSelected.on("change", function () {
        var currentValue = $(this).val();
        console.log(currentValue);
        if (currentValue == "A") {
            toggleBoxASection(true);
            toggleBoxBSection(false);
        }
        else {
            toggleBoxASection(false);
            toggleBoxBSection(true);
        }
    });
});