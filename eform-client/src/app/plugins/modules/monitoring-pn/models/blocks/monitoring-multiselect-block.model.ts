import {MonitoringBaseDataItem} from './monitoring-base-data-item.model';
import {KeyValuePairDto} from '../../../../../common/models/dto';

export class MonitoringMultiSelectBlock extends MonitoringBaseDataItem {
  keyValuePairList: KeyValuePairDto;
}
