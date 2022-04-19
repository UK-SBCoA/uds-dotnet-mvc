$(function() {
  /*
    INSTRUCTIONS:
    1. on the html provide the necessary data-attributes on the radio button inputs: 
    <input type="radio" data-toggle-values="[3,4,5,6,8,9]" data-toggle-targets="Learned" id="ReferralSource[1]" value="1" name="ReferralSource">
    * data-toggle-values = the values that will disable the data-toggle-target fields

    2. if you have to target more than one element to disable, then seperate by a comma (,)
    <input type="radio" data-toggle-values="[3,4,5,6,8,9]" data-toggle-targets="Learned,Test,Other" id="ReferralSource[1]" value="1" name="ReferralSource">

    3. in order for "other" text input fields to be disabled on load, add "disabled" attribute to all text inputs with the desired functionality
  */


   //get all checked values and disable/enable related fields
  $(`input[type="radio"]:checked`).map(function() {
    SectionToggle($(this))
  })

  //get all unchecked values and pre disable fields that require it. Ex: Other input fields within radio groups
  

  function SectionToggle(element) { 
    var fieldDisabled = false;

    //toggle with integer values
    if(element.data('toggle-values')) {
      var toggleTargetArray = element.data('toggle-targets').split(',');

      if(element.data('toggle-values').includes(parseInt(element.attr('value')))) {
        fieldDisabled = true;
      } else {
        fieldDisabled = false;
      }
      
      toggleTargetArray.forEach(target => {
        //disable radio inputs
        $(`input[name="${target}"]`).prop('disabled', fieldDisabled);
        //disable/enable text / number inputs if any exist
        $(`input[name="${target}Other"]`).prop('disabled', fieldDisabled);
      });
    }

    //toggle with boolean values
    if(element.data('toggle-bool')) {
      var toggleTargetArray = element.data('toggle-targets').split(',');

      if(element.data('toggle-bool').toString() == element.attr('value')) {
        fieldDisabled = true;
      } else {
        fieldDisabled = false;
      }
      
      toggleTargetArray.forEach(target => {
        //disable radio inputs
        $(`input[name="${target}"]`).prop('disabled', fieldDisabled);
        //disable/enable text / number inputs if any exist
        $(`input[name="${target}Other"]`).prop('disabled', fieldDisabled);
      });
    }
  }

  $(`input[type="radio"]`).on("change", function() {
    SectionToggle($(this));
    // loop through check fields after sectionToggle for any related fields effected by toggle
    $(`input[type="radio"]:checked`).map(function() {
      SectionToggle($(this))
    })
    // loop through disabled fields and disable any fields with valid values, but within disabled groups
    $(`input[type="radio"]:disabled`).map(function() {
      $(`input[name="${this.name}Other"]`).prop('disabled', true);
    })
  })

  //run code on checkbox changes
  $(`input[type="checkbox"]`).on("change", function() {
    $(`input[type="radio"]:checked`).map(function() {
      SectionToggle($(this))
    })
  })
})