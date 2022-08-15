/// <reference path="../lib/jquery/dist/jquery.js" />
$(() => {
  const radioConfig = [
    {
      'weight': 0,
      'name': 'Race',
      'disableExceptions': [50, 88, 99]
    },
    {
      'weight': 1,
      'name': 'SecondaryRace',
      'disableExceptions': [50, 88, 99]
    },
    {
      'weight': 2,
      'name': 'AdditionalRace',
      'disableExceptions': [50, 88, 99]
    }
  ]
  const isNewCoParticipant = $('input[name=IsNewCoParticipant]');
  const isFollowUpVisit = isNewCoParticipant.length > 0;
  const race = $('input[name=Race]');
  const raceChecked = $('input[name=Race]:checked');
  const secondaryRace = $('input[name=SecondaryRace]');
  const additionalRace = $('input[name=AdditionalRace]');
  const secondaryRaceChecked = $('input[name=SecondaryRace]:checked');
  const secondaryRaceNotReported = $('input[name=SecondaryRace][value=88]');
  const additionalRaceNotReported = $('input[name=AdditionalRace][value=88]');
  const subsequentRadioControl = new SubsequentRadioControlDisable(radioConfig);
  function disableAllRadioControlsExceptOne(controlNameToDisable, controlValueToEnable) {
    $(`input[name=${controlNameToDisable}]`).toArray().forEach(currentControl => {
      const currentRadioControl = $(currentControl);
      const shouldDisableControl = currentRadioControl.val() != controlValueToEnable;
      if (shouldDisableControl) {
        currentRadioControl.prop('disabled', true);
      }
    });
  }
  function setUnknownOrNotReportedStates() {
    if (race.is(':checked')) {
      if (raceChecked.val() == 99) {
        secondaryRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(secondaryRaceNotReported.prop('name'), 88);
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }
    if (secondaryRace.is(':checked')) {
      if (secondaryRaceChecked.val() == 99) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
      if (secondaryRaceChecked.val() == 88) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }
  }
  if (isFollowUpVisit) {
    isNewCoParticipant.on('change', (event) => {
      const isNewCoParticipantControl = $(event.target);
      if (isNewCoParticipantControl.is(':checked')) {
        if (isNewCoParticipantControl.val() == 'true') {
          race.each((_index, control) => $(control).prop('disabled', false));
          subsequentRadioControl.runRadioDisable();
          setUnknownOrNotReportedStates();
        }
        else {
          race.each((_index, control) => $(control).prop('disabled', true));
          secondaryRace.each((_index, control) => $(control).prop('disabled', true));
          additionalRace.each((_index, control) => $(control).prop('disabled', true));
        }
      }
    }).trigger('change');
  }
  setUnknownOrNotReportedStates();
  race.on('change', (event) => {
    const selectedRace = $(event.target);
    if (selectedRace.is(':checked')) {
      if (selectedRace.val() == 99) {
        secondaryRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(secondaryRaceNotReported.prop('name'), 88);
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }
    if (secondaryRaceChecked != null) {
      if (secondaryRaceChecked.val() == 99) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
      if (secondaryRaceChecked.val() == 88) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }
  });
  secondaryRace.on('change', (event) => {
    const selectedRace = $(event.target);
    if (selectedRace.is(':checked')) {
      if (selectedRace.val() == 99) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
      if (selectedRace.val() == 88) {
        additionalRaceNotReported.prop('checked', true);
        disableAllRadioControlsExceptOne(additionalRaceNotReported.prop('name'), 88);
      }
    }
  });
  /*
     INSTRUCTIONS:
     1. on the html provide the necessary data-attributes on the radio button inputs: 
     <input type="radio" data-toggle-values="[3,4,5,6,8,9]" data-toggle-targets="Learned" id="ReferralSource[1]" value="1" name="ReferralSource">
     * data-toggle-values = the values that will disable the data-toggle-target fields
 
     2. if you have to target more than one element to disable, then seperate by a comma (,)
     <input type="radio" data-toggle-values="[3,4,5,6,8,9]" data-toggle-targets="Learned,Test,Other" id="ReferralSource[1]" value="1" name="ReferralSource">
 
     3. in order for "other" text input fields to be disabled on load, add "disabled" attribute to all text inputs with the desired functionality
     NOTE: This is a refactor of the original radio toggle library, with a modification to check for partially disabled radio button groups
   */
  function disableRadioGroup(target, willDisable) {
    $(`input[name="${target}"]`).prop('disabled', willDisable);
    $(`input[name="${target}Other"]`).prop('disabled', willDisable);
  }
  function hasToggleValue(element) {
    return element.data('toggle-values').includes(parseInt(element.attr('value')));
  }
  function isDisabledRadioGroup(groupName) {
    const radioGroupArray = $(`input[name=${groupName}]`);
    const disabledButtons = radioGroupArray.filter(x => $(x).attr('disabled'));
    return disabledButtons == radioGroupArray.length;
  }
  function SectionToggle(element) {
    let shouldDisable = false;
    if (element.data('toggle-values')) {
      let toggleTargetArray = element.data('toggle-targets').split(',');
      if (toggleTargetArray.length > 0) {
        shouldDisable = hasToggleValue(element);
        toggleTargetArray.forEach(target => disableRadioGroup(target, shouldDisable));
      } else {
        console.error(`There we're no toggle targets provided for ${element.attr('name')}`);
      }
    }
    if (element.data('toggle-bool')) {
      let toggleTargetArray = element.data('toggle-targets').split(',');
      shouldDisable = element.data('toggle-bool').toString() == element.attr('value');
      toggleTargetArray.forEach(target => disableRadioGroup(target, shouldDisable));
    }
  }
  //get all checked values and disable/enable related fields
  $(`input[type="radio"]:checked`).map((_index, currentlyCheckedRadio) => {
    const checkedRadio = $(currentlyCheckedRadio);
    SectionToggle(checkedRadio);
  });
  $(`input[type="radio"]`).on("change", (event) => {
    const changedRadio = $(event.target);
    SectionToggle(changedRadio);
    // loop through check fields after sectionToggle for any related fields effected by toggle
    $(`input[type="radio"]:checked`).map((_index, currentlyCheckedRadio) => {
      const checkedRadio = $(currentlyCheckedRadio);
      SectionToggle(checkedRadio);
    });
    // loop through disabled fields and disable any fields with valid values, but within disabled groups
    $(`input[type="radio"]:disabled`).map((_index, disabledRadio) => {
      // checks if entire radio group is disabled, prevents partially disabled groups from having their other text field disabled automatically
      if (isDisabledRadioGroup(disableRadioGroup.name)) {
        $(`input[name="${disabledRadio.name}Other"]`).prop('disabled', true);
      }
    });
  });

  //run code on checkbox changes
  $(`input[type="checkbox"]`).on("change", (event) => {
    $(`input[type="radio"]:checked`).map(function () {
      SectionToggle($(event.target))
    })
  });
});