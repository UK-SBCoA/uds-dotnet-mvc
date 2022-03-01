$(function() {

  var skipSelected = false;

  var toggleSkipFields =(elem)=> {
    if($(elem).data("skip-targets")) {
      var skipTargets = $(elem).data('skip-targets').split(',');
      
      skipTargets.forEach((target) => {
        $(`input[name="${target}"]`).prop('disabled', skipSelected)
      })

      skipSelected = !skipSelected
    }
  }

  
  if($(`input[data-toggle-skip]:checked`).val()) {
    skipSelected = true;
  } else {
    skipSelected = false;
  }

  toggleSkipFields($('input[data-toggle-skip]'))

  $(`input[data-toggle-skip]`).on("change", function() {
   toggleSkipFields($(this))
  })

})