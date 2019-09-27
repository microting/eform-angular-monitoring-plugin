import {MonitoringBaseDataItem} from './monitoring-base-data-item.model';
import {KeyValuePairDto} from '../../../../../common/models/dto';

export class MonitoringSingleSelectBlock extends MonitoringBaseDataItem {
  keyValuePairList: KeyValuePairDto;
}
