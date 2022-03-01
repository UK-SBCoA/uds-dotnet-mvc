$(function () {
    /*
      INSTRUCTIONS:
      1. on the html provide the necessary data-attributes on the radio button inputs: 
      <input type="checkbox" data-toggle-values="[true]" data-toggle-targets="Learned" id="ReferralSource" value="true" name="ReferralSource">
      * data-toggle-values = the values that will disable the data-toggle-target fields
  
      2. if you have to target more than one element to disable, then seperate by a comma (,)
      <input type="checkbox" data-toggle-values="[true]" data-toggle-targets="Learned,Test,Other" id="ReferralSource" value="true" name="ReferralSource">
  
      3. in order for "other" text input fields to be disabled on load, add "disabled" attribute to all text inputs with the desired functionality
    */

    
    // initialize all checked values and disable/enable related fields
    $('input[type="checkbox"]:checked').each(function () {
        SectionToggle($(this))
    });

    function SectionToggle(element) {
        var fieldDisabled = false;

        //toggle with integer values
        if (element.data('toggle-values')) {
            var toggleTargetArray = element.data('toggle-targets').split(',');

            if (element.data('toggle-values').includes(element.is(':checked'))) {
                fieldDisabled = true;
            } else {
                fieldDisabled = false;
            }

            toggleTargetArray.forEach(target => {
                //disable target inputs

                $('input[name="' + target + '"]').prop("disabled", fieldDisabled);
                console.log("disable input " + target + " disabled prop to " + fieldDisabled);
            });
        }
    }

    $('input[type="checkbox"]').on("change", function () {
        SectionToggle($(this));
    });
    
})