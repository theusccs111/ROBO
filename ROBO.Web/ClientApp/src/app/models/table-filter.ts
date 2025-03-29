import { NzTableSortOrder } from "ng-zorro-antd/table";

export interface TableFilter {
    page: number;
    quantityRecords: number;
    sortFieldId: string | null;
    sortOrder: NzTableSortOrder | null;
  }