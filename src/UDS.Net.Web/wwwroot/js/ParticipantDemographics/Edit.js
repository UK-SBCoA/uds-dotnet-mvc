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
  ]
  new SubsequentRadioControlDisable(radioConfig);
  const race = $('input[name=Race]')
  const secondaryRace = $('input[name=SecondaryRace]')
  const secondaryRaceNotReported = $('input[name=SecondaryRace][value=88]');
  const additionalRaceNotReported = $('input[name=AdditionalRace][value=88]');
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