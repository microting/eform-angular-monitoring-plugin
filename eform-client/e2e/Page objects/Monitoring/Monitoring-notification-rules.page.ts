import Page from '../Page';
import XMLForeFormExtended from '../../Constants/XMLForeFormExtended';
import myEformsPage from '../MyEforms.page';

export class MonitoringNotificationRulesPage extends Page {
  constructor() {
    super();
  }

  public get MonitoringDropdown() {
    const ele = $('#monitoring-pn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get monitoringPnCalendar() {
    const ele = $('#monitoring-pn-calendar');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get RuleCreateBtn() {
    const ele = $('#ruleCreateBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get templateSelector() {
    const ele = $('#templateSelector');
    return ele;
  }

  public get DataFieldSelector() {
    const ele = $('#dataFieldSelector');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get CheckboxOption() {
    const ele = $('#selected');
    return ele;
  }

  public get GreaterThanValue() {
    const ele = $('#greaterThanValue');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get LessThanValue() {
    const ele = $('#lessThanValue');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get EqualValue() {
    const ele = $('#equalValue');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get EmailSubject() {
    const ele = $('#emailSubject');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get EmailTextArea() {
    const ele = $('#emailText');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get AttachReportBox() {
    const ele = $('#newEFormBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get RecipientEmail() {
    const ele = $('#newEFormBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get AddRecipientBtn() {
    const ele = $('#addRecipientBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get RuleSaveBtn() {
    const ele = $('#ruleEditSaveBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  public get RuleCancelBtn() {
    const ele = $('#ruleEditCancelBtn');
    ele.waitForDisplayed({ timeout: 20000 });
    ele.waitForClickable({ timeout: 20000 });
    return ele;
  }

  createNewEform(eFormLabel, selectableReplace, searchableReplace) {
    myEformsPage.newEformBtn.click();
    myEformsPage.xmlTextArea.waitForDisplayed({ timeout: 20000 });
    // Create replaced xml and insert it in textarea
    let xml = XMLForeFormExtended.XML.replace('TEST_LABEL', eFormLabel);
    xml = xml.replace('REPLACE_SEARCHABLE_ID', searchableReplace);
    xml = xml.replace('REPLACE_SEARCHABLE_ID', searchableReplace);
    xml = xml.replace('REPLACE_SINGLE_SELECT_SEARCH_ID', selectableReplace);
    xml = xml.replace('REPLACE_SINGLE_SELECT_SEARCH_ID', selectableReplace);
    browser.execute(function (xmlText) {
      (<HTMLInputElement>document.getElementById('eFormXml')).value = xmlText;
    }, xml);
    myEformsPage.xmlTextArea.addValue(' ');
    myEformsPage.createEformTagSelector.click();
    myEformsPage.createEformBtn.waitForDisplayed({ timeout: 10000 });
    // browser.pause(5000);
    myEformsPage.createEformBtn.click();
    // browser.pause(14000);
    myEformsPage.newEformBtn.waitForDisplayed({ timeout: 20000 });
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
    this.monitoringPnCalendar.click();
  }

  public createNewMonitoringRuleWithCheckbox(
    template,
    dataField,
    emailSubject,
    emailText,
    recipient
  ) {
    const spinnerAnimation = $('#spinner-animation');
    this.RuleCreateBtn.click();
    $('#templateSelector').waitForDisplayed({ timeout: 20000 });
    this.templateSelector.click();
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.selectOption(template);
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.DataFieldSelector.click();
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.selectOption(dataField);
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.CheckboxOption.click();
    this.EmailSubject.addValue(emailSubject);
    this.EmailTextArea.addValue(emailText);
    this.AttachReportBox.click();
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.RecipientEmail.addValue(recipient);
    this.AddRecipientBtn.click();
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    this.RuleSaveBtn.click();
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
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
