export class HttpError extends Error {
    public status: number;
    public validationErrors: Array<string>;
    
    constructor(message: string, status: number, validationErrors: Array<string> = null) {
        super(message);
        this.message = message;
        this.name = 'HttpError';
        this.status = status;
        this.validationErrors = validationErrors;
    }
}