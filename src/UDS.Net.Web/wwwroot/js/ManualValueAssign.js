//INSTRUCTIONS

//1. Add data attribute of "data_value_assign_targets" to the element you would like to be the trigger and set equal to string of names to target seperated by a comma
//example: data_value_assign_targets = "Speech,SpeechUntestable,FacialExpression"

//2. Add data attribute of data_value_assign_values to the element you would like to be the trigger and set equal to string of values to set seperated by a comma
//   The values will need to be in the order listed in the data_value_assign_targets
//example: data_value_assign_values = "1,2,3"

//3. Accepted values are:
//text = "text", for setting input fields
//integer = "1", for setting radio options
//null = will disable the field

$(() => {
    class ManualValueAssign {
        constructor(parentElement) {
            this.manualAssignParent = parentElement;
            this.targets = this.manualAssignParent.attr("data-value-assign-targets").split(",");
            this.values = this.manualAssignParent.attr("data-value-assign-values").split(",");
            this.parentValueActive = this.SetManualAssignParentValue(this.manualAssignParent);
            this.firstLoadDone = false;
        }

        SetManualAssignParentValue = (parentElement) => {
            if ($(parentElement).is(":checked")) {
                return true;
            } else {
                return false;
            }
        }

        CheckDataLengthOnLoad(targets, values) {
            if (targets.length != values.length) {
                this.InvalidDataLengthError(this.manualAssignParent, this.targets, this.values);
            }
        }

        SetDisabledOnLoad = (targets, values) => {
            targets.forEach((target, index) => {
                if (values[index] === "null") {
                    $(`input[name="${target}"]`).prop("disabled", this.parentValueActive);
                }
            })
        }

        SetDisable = (target, parentValueActive) => {
            $(`input[name="${target}"]`).prop("disabled", parentValueActive);
        }

        SetValue = (target, value) => {
            if ($(`input[name="${target}"]`).is('input:radio')) {
                $(`input[name="${target}"][value="${value}"]`).prop('checked', true);
            }

            if ($(`input[name="${target}"]`).is('input:text')) {
                $(`input[name="${target}"]`).val(value)
            }
        }

        InvalidDataLengthError(manualAssignParent, targets, values) {
            console.error(`The length of the data attributes targets and values on the manual assign parent element ${manualAssignParent} are not equal. Targets length is: ${targets.length} and values length is: ${values.length}. Please double check attribute value lengths and fix error before the manual value assign can be run`)
        }

        CheckQuestions = (targets, values) => {
            if (targets.length == values.length) {
                this.parentValueActive = this.SetManualAssignParentValue(this.manualAssignParent);

                targets.forEach((target, index) => {
                    if (values[index] === "null") {
                        this.SetDisable(target, this.parentValueActive)
                    }
                    
                    if (values[index] != "null") {
                        this.SetValue(target, values[index])
                    }
                })
            } else {
                this.InvalidDataLengthError(this.manualAssignParent, this.targets, this.values);
            }
        }
    }

    let manualValueAssign = new ManualValueAssign($('[data-value-assign-targets]'));
    manualValueAssign.SetDisabledOnLoad(manualValueAssign.targets, manualValueAssign.values)
    manualValueAssign.CheckDataLengthOnLoad(manualValueAssign.targets, manualValueAssign.values)

    $('[data-value-assign-targets]').on("change", () => {
        if ($('[data-value-assign-targets]').is(':checked')) {
            manualValueAssign.CheckQuestions(manualValueAssign.targets, manualValueAssign.values);
        }
    })
})