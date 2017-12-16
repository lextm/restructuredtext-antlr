grammar ReStructuredText;

/* https://stackoverflow.com/questions/6178546/antlr-grammar-for-restructuredtext-rule-priorities */

options {
  language=CSharp;
}

parse
  :  (paragraph | empty_line) + EOF
  ;

paragraph
  :  Comment? line+
  ;

line
  :  indentation? text
  ;

empty_line
  : Space* LineBreak
  ;

indentation
  :  Space+
  ;

text
  : text_fragment+ LineBreak
  ;

text_fragment
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
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ Star
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
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~BackTick+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

Comment
  :  '..'
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
  ;

Any
  :  .
  ;