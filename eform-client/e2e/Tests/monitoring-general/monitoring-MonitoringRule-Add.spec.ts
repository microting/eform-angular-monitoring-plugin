import rulesPage from '../../Page objects/Monitoring/Monitoring-notification-rules.page';
import loginPage from '../../Page objects/Login.page';
import myEformsPage from '../../Page objects/MyEforms.page';
import searchableLists from '../../Page objects/SearchableLists.page';
import selectableLists from '../../Page objects/SelectableLists.page';
import { generateRandmString } from '../../Helpers/helper-functions';
const expect = require('chai').expect;

describe('Monitoring - Monitoring Rules - Add', function () {
  before(function () {
    loginPage.open('/auth');
    loginPage.login();
  });
  it('should create a searchable, selectable list and eForm', function () {
    const spinnerAnimation = $('#spinner-animation');
    myEformsPage.Navbar.goToEntitySearch();
    searchableLists.createSearchableList_NoItem('Searchable');
    const searchableList = searchableLists.getFirstRowObject();
    const searchableId = searchableList.id.getText();
    myEformsPage.Navbar.goToEntitySelect();
    selectableLists.createSelectableList({ name: 'Selectable' });
    const selectableList = selectableLists.getFirstSelectableListObject();
    const selectableId = selectableList.id;
    loginPage.open('/');
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
    rulesPage.createNewEform('Number 1', selectableId, searchableId);
    spinnerAnimation.waitForDisplayed({ timeout: 90000, reverse: true });
  });
  it('should go to monitoring rules page', function () {
    rulesPage.goToMonitoringRulesPage();
    $('#spinner-animation').waitForDisplayed({ timeout: 90000, reverse: true });
  });
  it('should create a new rule with checkbox', function () {
    const template = 'Test e-mail notifikation på alle felter';
    const dataField = '2. Sæt flueben';
    const emailSubject = generateRandmString();
    const emailText = generateRandmString();
    const recipient = 'hej@hej.com';
    rulesPage.createNewMonitoringRuleWithCheckbox(
      template,
      dataField,
      emailSubject,
      emailText,
      recipient
    );
    const rule = rulesPage.getFirstRowObject(1);
    expect(rule.id).equal(1);
    expect(rule.eFormName).equal(template);
    expect(rule.trigger).equal('2. Sæt flueben = Checked');
    expect(rule.event).equal('Email');
  });
});
