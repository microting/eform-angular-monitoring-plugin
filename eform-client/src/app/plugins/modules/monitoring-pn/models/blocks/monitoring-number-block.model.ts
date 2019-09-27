import {MonitoringBaseDataItem} from './monitoring-base-data-item.model';
import {MonitoringOperatorType} from '../../enums/monitoring-operator-type.enum';

export class MonitoringNumberBlock extends MonitoringBaseDataItem {
  value: number;
  type: MonitoringOperatorType;
}
