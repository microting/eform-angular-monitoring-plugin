import {ChangeDetectorRef, Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {EFormService} from '../../../../../../common/services/eform';
import {MonitoringPnNotificationRulesService} from '../../../services';
import {NotificationRuleModel, RecipientModel} from '../../../models';
import {debounceTime, switchMap} from 'rxjs/operators';
import {FieldDto} from '../../../../../../common/models/dto/field.dto';
import {TemplateRequestModel} from '../../../../../../common/models/eforms';
import {TemplateDto} from '../../../../../../common/models/dto';
import {SupportedFieldTypes} from '../../../const';
import {BaseDataItem, CheckBoxBlock, SelectBlock, NumberBlock} from '../../../models/blocks';

@Component({
  selector: 'app-monitoring-pn-notification-rules-edit',
  templateUrl: './notification-rules-edit.component.html',
  styleUrls: ['./notification-rules-edit.component.scss']
})
export class NotificationRulesEditComponent implements OnInit {
  @ViewChild('frame') frame;
  @Output() ruleSaved: EventEmitter<void> = new EventEmitter<void>();

  templateTypeahead = new EventEmitter<string>();
  recipientEmail: string;
  recipients: RecipientModel[] = [];
  spinnerStatus = false;

  // Models
  ruleModel: NotificationRuleModel = new NotificationRuleModel();
  templateRequestModel: TemplateRequestModel = new TemplateRequestModel();
  templates: TemplateDto[] = [];
  selectedTemplate: TemplateDto = new TemplateDto();
  fields: FieldDto[] = [];
  selectedField: FieldDto = new FieldDto();

  get supportedFieldTypes() {
    return SupportedFieldTypes;
  }

  constructor(
    private monitoringRulesService: MonitoringPnNotificationRulesService,
    private eFormService: EFormService,
    private cd: ChangeDetectorRef
  ) {
    this.templateTypeahead
      .pipe(
        debounceTime(200),
        switchMap(term => {
          this.templateRequestModel.nameFilter = term;
          return this.eFormService.getAll(this.templateRequestModel);
        })
      )
      .subscribe(items => {
        this.templates = items.model.templates;
        this.cd.markForCheck();
      });
  }

  ngOnInit() {
  }

  onClose() {
    this.ruleModel = new NotificationRuleModel();
    this.frame.hide();
  }

  onTemplateChange() {
    this.updateSelectedTemplate();
  }

  onFieldChange() {
    this.selectedField = this.fields.find(f => f.id === this.ruleModel.dataItemId);

    const baseDataItem = {
      label: this.selectedField.label,
      description: this.selectedField.description
    } as BaseDataItem;

    switch (this.selectedField.fieldType) {
      case SupportedFieldTypes.CheckBox:
        this.ruleModel.data = {
          ...baseDataItem,
          defaultValue: false,
          selected: false
        } as CheckBoxBlock;
        break;
      case SupportedFieldTypes.Number:
        this.ruleModel.data = baseDataItem as NumberBlock;
        break;
      case SupportedFieldTypes.SingleSelect:
      case SupportedFieldTypes.MultiSelect:
      case SupportedFieldTypes.EntitySearch:
      case SupportedFieldTypes.EntitySelect:
        this.ruleModel.data = {
          ...baseDataItem,
          keyValuePairList: this.selectedField.keyValuePairList
        } as SelectBlock;
        break;
    }
  }

  show(id?: number) {
    if (id) {
      this.getSelectedRule(id);
    } else {
      this.ruleModel = {
        id: null,
        attachReport: false,
        data: null,
        dataItemId: null,
        recipients: [],
        ruleType: null,
        subject: '',
        templateId: null,
        text: ''
      };
    }
    this.frame.show();
  }

  getSelectedRule(id: number) {
    this.spinnerStatus = true;
    this.monitoringRulesService.getRule(id).subscribe((data) => {
      if (data && data.success) {
        this.ruleModel = data.model;
        this.updateSelectedTemplate();
      }
      this.spinnerStatus = false;
    });
  }

  saveRule() {
    this.spinnerStatus = true;

    if (this.ruleModel.id) {
      this.monitoringRulesService.updateRule(this.ruleModel).subscribe((data) => {
        if (data && data.success) {
          this.ruleSaved.emit();
          this.ruleModel = new NotificationRuleModel();
          this.frame.hide();
        }
        this.spinnerStatus = false;
      });
    } else {
      this.monitoringRulesService.createRule(this.ruleModel).subscribe((data) => {
        if (data && data.success) {
          this.ruleSaved.emit();
          this.ruleModel = new NotificationRuleModel();
          this.frame.hide();
        }
        this.spinnerStatus = false;
      });
    }
  }

  updateSelectedTemplate() {
    if (!this.ruleModel.templateId) {
      return;
    }

    this.eFormService.getSingle(this.ruleModel.templateId).subscribe(operation => {
      if (operation && operation.success) {
        this.selectedTemplate = operation.model;
        this.templates = [this.selectedTemplate];
        this.cd.markForCheck();
      }
    });

    this.eFormService.getFields(this.ruleModel.templateId).subscribe(operation => {
      if (operation && operation.success) {
        this.fields = operation.model;
      }
    });
  }

  addNewRecipient() {
    this.recipients.push({email: this.recipientEmail});
    this.recipientEmail = '';
  }

  removeRecipient(i: number) {
    this.recipients.splice(i, 1);
  }

  asCheckboxBlock(item: BaseDataItem) {
    return item as CheckBoxBlock;
  }

  asNumberBlock(item: BaseDataItem) {
    return item as NumberBlock;
  }

  asSelectBlock(item: BaseDataItem) {
    return item as SelectBlock;
  }
}
