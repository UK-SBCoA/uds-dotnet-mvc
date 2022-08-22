$(() => {
  class FormValidation {
      constructor() {
          this.eightA = $('input[name="TrailMakingTestASecondsToComplete"]');
          this.eightATwo = $('input[name="TrailMakingTestANumberOfCorrect"]');
          this.eightB = $('input[name="TrailMakingTestBSecondsToComplete"]');
          this.eightBTwo = $('input[name="TrailMakingTestBNumberOfCorrect"]');
          this.trailMakingErrorA = $('input.trailMakingErrorA');
          this.trailMakingErrorB = $('input.trailMakingErrorB');
          this.moCAPartAdministered = $('input[name="MoCAPartAdministered"]');

          this.scoreList =
              [
                  "MoCATrials",
                  "MoCACube",
                  "MoCAClockContour",
                  "MoCAClockNumbers",
                  "MoCAHands",
                  "MoCANaming",
                  "MoCADigits",
                  "MoCALetter",
                  "MoCASerial",
                  "MoCARepetition",
                  "MoCAFluency",
                  "MoCAAbstraction",
                  "MoCANoCue",
                  "MoCADate",
                  "MoCAMonth",
                  "MoCAYear",
                  "MoCADay",
                  "MoCAPlace",
                  "MoCACity"
              ]
      }
      TrailMakingSecondsCompleteCheck(inputName) {
          const inputControl = $(`input[name=${inputName}]`);
          $.ajax({
              url:`${window.location.origin}/NeuropsychologicalBatteryScores/${inputName}ValidateRange`,
              data: {
                  seconds: inputControl.val()
              },
              success: (result) => {
                  let errorSpanElement = $(`.${inputName}`)
                  // Not Valid set message;
                  console.log(`${inputName}:${result}`)
                  if(!result) {
                      console.log('enable error');
                      if(errorSpanElement.hasClass('errorHidden'))
                      {
                          console.log('remove class, show error')
                      errorSpanElement.removeClass('errorHidden'); 
                      }
                  } 
                  if(result) {
                      if(!errorSpanElement.hasClass('errorHidden'))
                      {
                      errorSpanElement.addClass('errorHidden'); 
                      }
                  }
              }
          });
      }
      TrailMakingSecondsNumberCompleteCheck(parentInputName, childInputName) {
          const parentInputControl = $(`input[name=${parentInputName}]`);
          const childInputControl = $(`input[name=${childInputName}]`);
          $.ajax({
              url:`${window.location.origin}/NeuropsychologicalBatteryScores/${childInputName}ValidateRange`,
              data: {
                  seconds: parentInputControl.val(),
                  correctLines: childInputControl.val()
              },
              success: (result) => {
                  let errorSpanElement = $(`.${childInputName}`)
                  // Not Valid set message;
                  console.log(`${childInputName}:${result}`)
                  if(!result) {
                      console.log('enable error');
                      if(errorSpanElement.hasClass('errorHidden'))
                      {
                          console.log('remove class, show error')
                      errorSpanElement.removeClass('errorHidden'); 
                      }
                  } 
                  if(result) {
                      if(!errorSpanElement.hasClass('errorHidden'))
                      {
                      errorSpanElement.addClass('errorHidden'); 
                      }
                  }
              }
          });
      }
      /*show/hide error text for value conflict*/
      TrailMakingErrorCheck = (errorElement, expectedValue, parentElement, parentElementMax) => {
          var inputField = $(`input[name="${errorElement}"`)
          var parentElementValue = $(`input[name="${parentElement}"`).val()
          var isZeroOrGreaterAndLessThanMax = (0 <= parentElementValue) && (parentElementValue < parentElementMax);
          if(isZeroOrGreaterAndLessThanMax) {
              inputField.val(expectedValue);
          }
          this.TrailMakingSecondsCompleteCheck(parentElement, parentElement);
          this.TrailMakingSecondsNumberCompleteCheck(parentElement, errorElement);
      }
      //add all of the values for 1f - 1aa and compare to total raw score
      TotalRawScoreSum = (sumChange) => {
          var scoreSum = 0;
          var totalRawScore = parseInt($('input[name="TotalRawScore"').val());

          this.scoreList.forEach((elem) => {
              var elemValue = parseInt($(`input[name="${elem}"]`).val())
              if (isNaN(elemValue)) {
                  elemValue = 0;
              }

              //if elem value is 88 or an error code, don't caclulate to total
              if (elemValue >= 88) {
                  elemValue = 0;
              } else {
                  scoreSum += elemValue;
              }
          })

          if (sumChange && totalRawScore != 88) {
              totalRawScore = scoreSum;
              $('input[name="TotalRawScore"]').val(totalRawScore);
          }

          //run function here
          this.ApplyTotalScoreErrorField(scoreSum, totalRawScore);
      }

      SetMoCAPartAdministeredCode = (elem) => {

          let moCAValue = elem.val();
          let disabled;

          if (moCAValue == "true") {
              disabled = true
          } else {
              disabled = false
          }

          $('input[name="MoCAPartAdministeredCode"]').prop('disabled', disabled);
      }

      ApplyTotalScoreErrorField = (scoreSum, totalRawScore) => {
          if (scoreSum != totalRawScore && totalRawScore != 88 && !isNaN(totalRawScore)) {
              $('.TotalRawScoreError').text(`The MoCA Item Scores you have entered equal ${scoreSum} and do not add up to the Total Raw Score of ${totalRawScore}.  Please check both the Item Scores and/or the Total Raw Score and make corrections.`)

              if ($('.TotalRawScoreError').hasClass("errorHidden")) {
                  $('.TotalRawScoreError').removeClass("errorHidden")
              }
              if (!$('input[name="TotalRawScore"]').hasClass("errorField")) {
                  $('input[name="TotalRawScore"]').addClass("errorField")
              }

              $('input[name="TotalRawScore"]').val(null);
          } else {
              if (!$('.TotalRawScoreError').hasClass("errorHidden")) {
                  $('.TotalRawScoreError').addClass("errorHidden")
                  $('input[name="TotalRawScore"]').removeClass("errorField")
              }
              if ($('input[name="TotalRawScore"]').hasClass("errorField")) {
                  $('input[name="TotalRawScore"]').removeClass("errorField")
              }
          }
      }
  }

  var formValidation = new FormValidation;

  //onload run functions
  /*on change functions
  on change, if 8a is null or < 150, 8a2 NEEDS to be 24. If 8a is null, 8a2 is disabled. Same with 8b and 8b2 */
  $(formValidation.eightA).on('change', (elem) => {
      formValidation.TrailMakingErrorCheck('TrailMakingTestANumberOfCorrect', 24, 'TrailMakingTestASecondsToComplete', 150)
  });

  $(formValidation.eightB).on('change', (elem) => {
      formValidation.TrailMakingErrorCheck('TrailMakingTestBNumberOfCorrect', 24, 'TrailMakingTestBSecondsToComplete', 300)
  });

  $(formValidation.eightATwo).on('change', () => {
      formValidation.TrailMakingErrorCheck('TrailMakingTestANumberOfCorrect', 24, 'TrailMakingTestASecondsToComplete', 150)
  })

  $(formValidation.eightBTwo).on('change',() => {
      formValidation.TrailMakingErrorCheck('TrailMakingTestBNumberOfCorrect', 24, 'TrailMakingTestBSecondsToComplete', 300)
  })

  $('.totalScoreSum').on('change',() => {
      formValidation.TotalRawScoreSum(true);
  });

  $('input[name="TotalRawScore"]').on('change', (elem) => {
      formValidation.totalRawScore = $(elem.target).val();
      formValidation.TotalRawScoreSum();
  });

  $('input[name="VerbalFluencyPhonemicTestGeneratedFWords"]').on('change', (elem) => {
      $('input[name="MoCALanguageFluencyNumberCorrect"]').val($(elem.target).val())
  })

  $(formValidation.moCAPartAdministered).change((elem) => {
      formValidation.SetMoCAPartAdministeredCode($(elem.target))
  })

  //onload count the scoreSum
  formValidation.TotalRawScoreSum();
  formValidation.SetMoCAPartAdministeredCode($('input[name="MoCAPartAdministered"]:checked'))
});