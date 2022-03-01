class RelationshipTable {
    _visitType = '';
    _tablePrefix = '';
    _enabledRows = '';
    constructor(tablePrefix) {
        this._visitType = $('input[id=Visit_VisitType]').val();
        this._tablePrefix = tablePrefix;
        this._enabledRows = this.GetRelationshipCount(tablePrefix);
        this.Initialize();
    }
    Initialize() {
        const totalRowCount = this.GetRowCount();
        if(this._tablePrefix != 'Parent') {
            const disableStart = parseInt(this._enabledRows) + 1;
            for(let i = disableStart; i <= totalRowCount; i++) {
                this.ReadOnlyRow(i);
            }
            // Setup Relationship Watch
            let disableTask;
            $(`input[name=${this._tablePrefix}Number`).on('keyup', (event) => {
                const newStart = $(event.target).val() ? (parseInt($(event.target).val()) + 1) : 0;
                clearTimeout(disableTask);
                disableTask = setTimeout(() => {
                    if(newStart >= 0) {
                        for(let i = newStart; i <= totalRowCount; i++) {
                            this.ReadOnlyRow(i);
                            this.ClearRowValues(i);
                        }
                        for(let i = 1; i <= newStart -1; i++) {
                            this.EnableRow(i);
                        }
                    }
                }, 500);     
            });
        }
        for(let i = 1; i <= totalRowCount; i++) {
            const neruoHasValue = this.GetRow(i).find('input[name$=PrimaryNeurologicalProblemPsychiatricCondition]').first().val();
            if(neruoHasValue) {
                this.EnableNeuroControls(i);
            }
            else {
                this.DisableNeuroControls(i);
            }
            this.AddNeruoPsychInputWatch(i);
        }
        if(this._visitType == 'FVP') {
            this.WatchRecentChange(this._tablePrefix);
            $(`input[name=${this._tablePrefix}Change]`).on('change', (event) => {
                this.WatchRecentChange(this._tablePrefix);
            });
        }
    }
    WatchRecentChange(prefix) {
        const tableChanger = $(`input[name=${prefix}Change]:checked`);
        if(tableChanger.is(':checked')) {
            if (tableChanger.val() == "1") {
                let end = 0;
                if (prefix != 'Parent') {
                    end = $(`input[name=${prefix}Number]`).val();
                } else {
                    end = 2;
                }
                for (let i = 1; i <= end; i++) {
                    this.EnableRow(i);
                    const neruoHasValue = this.GetRow(i).find('input[name$=PrimaryNeurologicalProblemPsychiatricCondition]').first().val();
                    if(neruoHasValue) {
                        this.EnableNeuroControls(i);
                    }
                }
            } else {
                // Disable Table
                $(`#${prefix}Table tr`).each((index, element) => {
                    this.ReadOnlyRow(index + 1);
                });
            }
        }
    }
    GetRelationshipCount(relationship) {
        if(relationship != 'Parent') {
            const relationshipCount = parseInt($(`input[name=${relationship}Number`).val() ? $(`input[name=${relationship}Number`).val() : 0);
            return relationshipCount;
        }
        return 2;
    }
    GetAllRows() {
        return $(`#${this._tablePrefix}Table > tbody tr`)
    }
    GetRowCount() {
        return this.GetAllRows().length;
    }
    GetRow(relationshipIndex) {
        return $(`#${this._tablePrefix}Row${relationshipIndex}`);
    }
    ReadOnlyRow(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        jRow.find('input').prop('readonly', true);
    }
    EnableRow(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        jRow.find('input').not('[data-neurocon]').prop('readonly', false);
    }
    ClearRowValues(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        jRow.find(':input:not([type=hidden])').val('');
    }
    EnableNeuroControls(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        jRow.find('input[data-neurocon]').prop('readonly', false);
    }
    DisableNeuroControls(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        jRow.find('input[data-neurocon]').prop('readonly', true);
    }
    AddNeruoPsychInputWatch(relationshipIndex) {
        const jRow = this.GetRow(relationshipIndex);
        const neuroWatch = jRow.find('input[name$="PrimaryNeurologicalProblemPsychiatricCondition"]').first();
        neuroWatch.on('keydown keyup', (events) => {
            if($(events.target).val() == '')
            {
                this.DisableNeuroControls(relationshipIndex);
            } else {
                this.EnableNeuroControls(relationshipIndex);
            }
        });
    }
}
class SubjectFamilyHistory {
    Initialize() {
        // Parent Table Special Case
        new RelationshipTable('Parent');
        new RelationshipTable('Sibling');
        new RelationshipTable('Children');
        this.SetupQuestions();
    }
    SetupQuestions() {
        var specifyControls = $('input[name$=Specify]');
        specifyControls.each((index, element) => {
            const childControl = $(element);
            const parentControl = $(element).attr('name').replace('_Specify', '');
            const ctrl = $(`input[name=${parentControl}]`)
            ctrl.on('change', (event) => {
                const control = $(event.target);
                if(control.is(':checked')) {
                    if(control.val() == 8) {
                        childControl.prop('readonly', false);
                    }
                    else if (control.val() == 1 && control.attr('name') == 'Other_Evidence') {
                        childControl.prop('readonly', false);
                    } 
                    else {
                        childControl.prop('readonly', true);
                    }
                }
            });
            ctrl.trigger('change');
        });
    }

}
$(() => {
    $('[data-toggle="tooltip"]').tooltip()
    var subjectFamilyHistory  = new SubjectFamilyHistory();
    subjectFamilyHistory.Initialize();
});