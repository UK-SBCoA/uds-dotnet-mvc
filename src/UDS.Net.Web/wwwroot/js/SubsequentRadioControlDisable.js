class SubsequentRadioControlDisable {
  _radioControlConfiguration = [];
  _numberOfControls;
  constructor(radioConfiguration) {
    this._radioControlConfiguration = radioConfiguration;
    this._numberOfControls = radioConfiguration.length;
    this.runRadioDisable();
    this.setupWatchForRadioButtons();
    
  }
  getRadioConfig(controlName) {
    return this._radioControlConfiguration.filter(x => x.name == controlName)[0];
  }
  disableRadioOption(controlName, value) {
    const controlToDisable = $(`input[name=${controlName}][type='radio'][value=${value}]`);
    controlToDisable.prop('disabled', true);
  }
  resetPreviouslyDisabledRadioGroup(controlName) {
    const radioGroupEnable = $(`input[name=${controlName}][type='radio']:disabled`).toArray();
    radioGroupEnable.forEach(x => $(x).prop('disabled', false));
  }
  getCheckedRadioValue(controlName) {
    return $(`input[name=${controlName}]:checked`).val()
  }
  setOtherTextDisableState(controlName, disabled) {
    const textBox = $(`input[name=${controlName}Other]`);
    textBox.prop('disabled', disabled);
  }
  getCurrentlyCheckedValue(controlName) {
    const checkedControls = $(`input[name=${controlName}][type='radio']:checked`);
    if(checkedControls.length > 0) {
      return checkedControls[0].val();
    }
    return -1;
  }
  runRadioDisable() {
    const previouslyCheckedValues = [];
    this._radioControlConfiguration.forEach(radioConfiguration => {
      if(radioConfiguration.hasOther) {
        const currentValue = this.getCheckedRadioValue(radioConfiguration.name);
        const radioGroupHasACheckedValue = currentValue >= 0
        if(radioGroupHasACheckedValue) {
          const otherTextBoxShouldBeEnabled = currentValue == radioConfiguration.otherEnableValue
          if(otherTextBoxShouldBeEnabled) {
            this.setOtherTextDisableState(radioConfiguration.name, false);
          } else {
            this.setOtherTextDisableState(radioConfiguration.name, true);
          }
        }
      }
      const isSubsequent = 0 < radioConfiguration.weight; 
      if(isSubsequent) {
        this.resetPreviouslyDisabledRadioGroup(radioConfiguration.name);
        previouslyCheckedValues.forEach(previouslyCheckedValue => {
          const isOtherValue = radioConfiguration.hasOther && previouslyCheckedValue == radioConfiguration.otherEnableValue;
          const isExceptionValue = radioConfiguration.disableExceptions.indexOf(+previouslyCheckedValue) > -1;
          if(!isOtherValue && !isExceptionValue) {
            this.disableRadioOption(radioConfiguration.name, previouslyCheckedValue);
          }
        });
      }
      previouslyCheckedValues.push(this.getCheckedRadioValue(radioConfiguration.name));
    });
  }
  setupWatchForRadioButtons() {
    this._radioControlConfiguration.forEach(radioConfiguration => {
      const radioControls = $(`input[name=${radioConfiguration.name}][type='radio']`).toArray();
      radioControls.forEach(control => {
        $(control).on('change', (event) => {
          const changedRadio = $(event.target)
          if(changedRadio.is(':checked')) {
            const changedRadioName = changedRadio.attr('name');
            const changedRadioConfig = this.getRadioConfig(changedRadioName);
            const isNotTheLastControl = changedRadioConfig.weight < this._numberOfControls - 1;
            const otherTextBoxShouldEnable = radioConfiguration.hasOther && changedRadio.val() == radioConfiguration.otherEnableValue
            if(otherTextBoxShouldEnable) {
              this.setOtherTextDisableState(changedRadioName, false);
            }
            if(isNotTheLastControl) {
              this.runRadioDisable();
            }
          }
        });
      });
    });
  }
}