export interface IShowData {
    showId: string;
    date: string;
    showName: string;
    tickets: ITicketStatus[] | null;
    seatAllocation: number

};

export interface IShowInformation {
    baseShowId: string;
    name: string;
    dates: string[];
    seatingAllocation: number;
} ;

export interface ITicketStatus {
    ticketId: string;
    sold: boolean;
};