import {RecipientModel} from './recipient.model';
import {NotificationRuleType} from '../const';
import {BaseDataItem} from './blocks';

export class NotificationRuleModel {
  id?: number;
  checkListId: number;
  subject: string;
  text: string;
  attachReport: boolean;
  ruleType: NotificationRuleType;
  dataItemId: number;
  data?: BaseDataItem;
  recipients: RecipientModel[];
}
