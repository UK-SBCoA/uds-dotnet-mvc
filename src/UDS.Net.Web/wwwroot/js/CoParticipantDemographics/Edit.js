// With the FVP, if #3 is false, then force disable 4a even if 4 was previously 1
/// <reference path="../../lib/jquery/dist/jquery.js" />
$(() => {
    const radioConfig = [
        {
          'weight': 0,
          'name': 'Race',
          'hasOther': true,
          'otherEnableValue': 50,
          'disableExceptions': [50, 88, 99]
        },
        {
          'weight': 1,
          'name': 'SecondaryRace',
          'hasOther': true,
          'otherEnableValue': 50,
          'disableExceptions': [50, 88, 99]
        },
        {
          'weight': 2,
          'name': 'AdditionalRace',
          'hasOther': true,
          'otherEnableValue': 50,
          'disableExceptions': [50, 88, 99]
        }
    ];
    const subsequentRadioControl = new SubsequentRadioControlDisable(radioConfig);
    $('input[name="IsNewCoParticipant"]').on('change', () => {
        $('input[name="EthnicOrigins"]').prop('disabled', true)
        $('input[name="HispanicLatinoEthnicity"]').prop('checked', false)
    });
    function disableAllRadioControlsExceptOne(controlNameToDisable, controlValueToEnable) {
      $(`input[name=${controlNameToDisable}]`).toArray().forEach(currentControl => {
        console.log(`Disabling control ${currentControl}`);
        const currentRadioControl = $(currentControl)
        const shouldDisableControl = currentRadioControl.val() != controlValueToEnable;
        if(shouldDisableControl) {
          currentRadioControl.prop('disabled', true);
        }
      });
    }
    const race = $('input[name=Race]')
    const secondaryRace = $('input[name=SecondaryRace]')
    const secondaryRaceNotReported = $('input[name=SecondaryRace][value=88]');
    const additionalRaceNotReported = $('input[name=AdditionalRace][value=88]');
    race.on('change', (event) => {
      const selectedRace = $(event.target);
      if(selectedRace.val() == 99) {
        secondaryRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(secondaryRaceNotReported.prop('name'), 88);
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }).trigger('change');
    secondaryRace.on('change', (event) => {
      const selectedRace = $(event.target);
      if(selectedRace.val() == 99) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }if(selectedRace.val() == 88) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }).trigger('change');
});
