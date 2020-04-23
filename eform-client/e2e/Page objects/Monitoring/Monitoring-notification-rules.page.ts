import Page from '../Page';
import XMLForEform from '../../Constants/XMLForEform';
import XMLForeFormExtended from '../../Constants/XMLForeFormExtended';
import XMLForEformFractions from '../../Constants/XMLForEformFractions';

export class MonitoringNotificationRulesPage extends Page {
    constructor() {
        super();
    }
    public get newEformBtn() {
        $('#newEFormBtn').waitForDisplayed({timeout: 20000});
        $('#newEFormBtn').waitForClickable({timeout: 20000});
        return $('#newEFormBtn');
    }
    public get createEformTagSelector() {
        $('#createEFormMultiSelector').waitForDisplayed({timeout: 20000});
        $('#createEFormMultiSelector').waitForClickable({timeout: 20000});
        return $('#createEFormMultiSelector');
    }

    public get createEformNewTagInput() {
        $('#addTagInput').waitForDisplayed({timeout: 20000});
        $('#addTagInput').waitForClickable({timeout: 20000});
        return $('#addTagInput');
    }

    public get xmlTextArea() {
        $('#eFormXml').waitForDisplayed({timeout: 20000});
        $('#eFormXml').waitForClickable({timeout: 20000});
        return $('#eFormXml');
    }

    public get createEformBtn() {
        $('#createEformBtn').waitForDisplayed({timeout: 20000});
        $('#createEformBtn').waitForClickable({timeout: 20000});
        return $('#createEformBtn');
    }
    public get MonitoringDropdown() {
        return $(`//*[@class= 'dropdown']//*[@id= '']`);
    }
    public get MonitoringRulesBtn() {
        return $(`//*[@id= 'monitoring-pn-calendar']`);
    }
    public get RuleCreateBtn() {
        $('#ruleCreateBtn').waitForDisplayed({timeout: 20000});
        $('#ruleCreateBtn').waitForClickable({timeout: 20000});
        return $('#ruleCreateBtn');
    }
    public get templateSelector() {
        return $(`//*[@id= 'templateSelector']`);
    }
    public get DataFieldSelector() {
        $('#dataFieldSelector').waitForDisplayed({timeout: 20000});
        $('#dataFieldSelector').waitForClickable({timeout: 20000});
        return $('#dataFieldSelector');
    }
    public get CheckboxOption() {
        return $(`//*[@id= 'selected']`);
    }
    public get GreaterThanValue() {
        $('#greaterThanValue').waitForDisplayed({timeout: 20000});
        $('#greaterThanValue').waitForClickable({timeout: 20000});
        return $('#greaterThanValue');
    }
    public get LessThanValue() {
        $('#lessThanValue').waitForDisplayed({timeout: 20000});
        $('#lessThanValue').waitForClickable({timeout: 20000});
        return $('#lessThanValue');
    }
    public get EqualValue() {
        $('#equalValue').waitForDisplayed({timeout: 20000});
        $('#equalValue').waitForClickable({timeout: 20000});
        return $('#equalValue');
    }
    public get EmailSubject() {
        $('#emailSubject').waitForDisplayed({timeout: 20000});
        $('#emailSubject').waitForClickable({timeout: 20000});
        return $('#emailSubject');
    }
    public get EmailTextArea() {
        $('#emailText').waitForDisplayed({timeout: 20000});
        $('#emailText').waitForClickable({timeout: 20000});
        return $('#emailText');
    }
    public get AttachReportBox() {
        $('#attachReport').waitForDisplayed({timeout: 20000});
        $('#attachReport').waitForClickable({timeout: 20000});
        return $('#attachReport');
    }
    public get RecipientEmail() {
        $('#recipientEmail').waitForDisplayed({timeout: 20000});
        $('#recipientEmail').waitForClickable({timeout: 20000});
        return $('#recipientEmail');
    }
    public get AddRecipientBtn() {
        $('#addRecipientBtn').waitForDisplayed({timeout: 20000});
        $('#addRecipientBtn').waitForClickable({timeout: 20000});
        return $('#addRecipientBtn');
    }
    public get RuleSaveBtn() {
        $('#ruleEditSaveBtn').waitForDisplayed({timeout: 20000});
        $('#ruleEditSaveBtn').waitForClickable({timeout: 20000});
        return $('#ruleEditSaveBtn');
    }
    public get RuleCancelBtn() {
        $('#ruleEditCancelBtn').waitForDisplayed({timeout: 20000});
        $('#ruleEditCancelBtn').waitForClickable({timeout: 20000});
        return $('#ruleEditCancelBtn');
    }

    createNewEform(eFormLabel, selectableReplace, searchableReplace) {
        this.newEformBtn.click();
        $('#eFormXml').waitForDisplayed({timeout: 20000});
        // Create replaced xml and insert it in textarea
        let xml = XMLForeFormExtended.XML.replace('TEST_LABEL', eFormLabel);
        xml = xml.replace('REPLACE_SEARCHABLE_ID', searchableReplace);
        xml = xml.replace('REPLACE_SEARCHABLE_ID', searchableReplace);
        xml = xml.replace('REPLACE_SINGLE_SELECT_SEARCH_ID', selectableReplace);
        xml = xml.replace('REPLACE_SINGLE_SELECT_SEARCH_ID', selectableReplace);
        browser.execute(function (xmlText) {
            (<HTMLInputElement>document.getElementById('eFormXml')).value = xmlText;
        }, xml);
        this.xmlTextArea.addValue(' ');
        this.createEformTagSelector.click();
        $('#createEformBtn').waitForDisplayed({timeout: 10000});
        // browser.pause(5000);
        this.createEformBtn.click();
        // browser.pause(14000);
        $('#delete-eform-btn').waitForDisplayed({timeout: 20000});
    }
    public selectOption(option) {
        $(`//*[text()="${option}"]`).click();
    }
    public getFirstRowObject(rowNum): RulesRowObject {
        browser.pause(500);
        return new RulesRowObject(rowNum);
    }
    public goToMonitoringRulesPage() {
        this.MonitoringDropdown.click();
        $('#monitoring-pn-calendar').waitForDisplayed({timeout: 20000});
        this.MonitoringRulesBtn.click();
        $('#ruleCreateBtn').waitForDisplayed({timeout: 20000});
    }
    public createNewMonitoringRuleWithCheckbox(template, dataField, emailSubject, emailText, recipient) {
        this.RuleCreateBtn.click();
        $('#templateSelector').waitForDisplayed({timeout: 20000});
        this.templateSelector.click();
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.selectOption(template);
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.DataFieldSelector.click();
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.selectOption(dataField);
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.CheckboxOption.click();
        this.EmailSubject.addValue(emailSubject);
        this.EmailTextArea.addValue(emailText);
        this.AttachReportBox.click();
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.RecipientEmail.addValue(recipient);
        this.AddRecipientBtn.click();
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
        this.RuleSaveBtn.click();
        $('#spinner-animation').waitForDisplayed({timeout: 90000, reverse: true});
    }
}

const rulesPage = new MonitoringNotificationRulesPage();
export default rulesPage;

export class RulesRowObject {
    constructor(rowNum) {
        if ($$('#ruleId')[rowNum - 1]) {
            this.id = $$('#ruleId')[rowNum - 1];
            try {
                this.eFormName = $$('#ruleeFormName')[rowNum - 1].getText();
            } catch (e) {}
            try {
                this.trigger = $$('#ruleTrigger')[rowNum - 1].getText();
            } catch (e) {}
            try {
                this.event = $$('#ruleEvent')[rowNum - 1].getText();
            } catch (e) {}
            try {
                this.editBtn = $$('#updateRuleBtn')[rowNum - 1];
            } catch (e) {}
            try {
                this.deleteBtn = $$('#deleteRuleBtn')[rowNum - 1];
            } catch (e) {}
        }
    }
    id;
    eFormName;
    trigger;
    event;
    editBtn;
    deleteBtn;
}
