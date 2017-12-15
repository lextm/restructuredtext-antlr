grammar restructuredtext;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  output=AST;
  backtrack=true;
  memoize=true;
}

tokens {
  ROOT;
  PARAGRAPH;
  INDENTATION;
  LINE;
  WORD;
  BOLD;
  ITALIC;
  INTERPRETED_TEXT;
  INLINE_LITERAL;
  REFERENCE;
}

parse
  :  paragraph+ EOF -> ^(ROOT paragraph+)
  ;

paragraph
  :  line+ -> ^(PARAGRAPH line+)
  |  Space* LineBreak -> /* omit line-breaks between paragraphs from AST */
  ;

line
  :  indentation text+ LineBreak -> ^(LINE text+)
  ;

indentation
  :  Space* -> ^(INDENTATION Space*)
  ;

text
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  Space
  |  Star
  |  EscapeSequence
  |  Any
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star -> ^(BOLD boldAtom+)
  ;  

italic
  :  Star italicAtom+ Star -> ^(ITALIC italicAtom+)
  ;

boldAtom
  :  ~(Star | LineBreak)
  |  italic
  ;

italicAtom
  :  ~(Star | LineBreak)
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick -> ^(INTERPRETED_TEXT interpretedTextAtoms)
  ;

interpretedTextAtoms
  :  ~BackTick+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick -> ^(INLINE_LITERAL inlineLiteralAtoms)
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore -> ^(REFERENCE Any+)
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  |  '\r'
  ;

Any
  :  .
  ;