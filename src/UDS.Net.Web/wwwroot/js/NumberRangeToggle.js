$(function() {
  /*
    INSTRUCTIONS:

  */

  $(`input[type="number"][data-toggle-range]`).map((index, elem) => {
    SectionToggle($(elem));
  })

  function SectionToggle(elem) {
    var fieldDisabled = false;

    if(elem.data('toggle-range')) {
      var startRange = parseInt(elem.data('toggle-range')[0]);
      var endRange = parseInt(elem.data('toggle-range')[1]);
      var elemValue = parseInt(elem.val());
      var toggleTargetArray = elem.data('toggle-targets').split(',');

      if(elemValue >= startRange && elemValue <= endRange) {
        fieldDisabled = true;
      }
      
      toggleTargetArray.forEach(target => {
        $(`input[name="${target}"]`).prop('disabled', fieldDisabled);
      });
    }
  }

  // devnote: look into why using on("change") works differently than in the .map function. The return var works differently
  $(`input[type="number"][data-toggle-range]`).on("change", function() {
    SectionToggle($(this));
  })
})